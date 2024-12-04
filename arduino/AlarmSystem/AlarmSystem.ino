#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <DHT.h>
#include <Crypto.h>
#include <SHA256.h>
#include <Base64.h>  // Uključivanje Base64 biblioteke

// Definicija pina i tipa senzora
#define DHTPIN D3     // Pin na koji je povezan DATA pin DHT11
#define DHTTYPE DHT11 // Tip senzora (DHT11)
#define PIRPIN D2     // Pin na koji je povezan signalni pin PIR senzora

DHT dht(DHTPIN, DHTTYPE); // Kreiranje objekta senzora

const char* ssid = "Mirnes";      
const char* password = "mirnes123";    
String deviceId = "5A25D07B-670D-47DE-BCBF-C18D26038549"; 
const char* header = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
String payload = "{\"sub\":\"" + deviceId + "\",\"iat\":1609459200,\"roles\":\"Device\"}";  // Koristimo String tip
const char* secretKey = "xCgmgpqPadKTaUgWF8j4YX3DZP7cgzYPSkG7GhJYbCXkroJVGMR3R6LFxRP1xwl5";  
String jwt = "";

// Funkcija za Base64 URL encoding (zamena + sa - i / sa _ i uklanjanje =)
String base64UrlEncode(const String& input) {
  String encoded = base64::encode((const uint8_t*)input.c_str(), input.length());  
  encoded.replace("+", "-");
  encoded.replace("/", "_");
  encoded.replace("=", "");  // Uklanjanje paddinga
  return encoded;
}

String createJWT(const char* key, const char* header, const String& payload) {
  // Base64 URL kodiranje header-a i payload-a
  String encodedHeader = base64UrlEncode(header);  // Koristimo Base64 URL encoding
  String encodedPayload = base64UrlEncode(payload);  // Koristimo Base64 URL encoding

  String message = encodedHeader + "." + encodedPayload;

  // HMAC SHA256
  SHA256 sha256;
  sha256.resetHMAC((const uint8_t*)key, strlen(key));
  sha256.update((const uint8_t*)message.c_str(), message.length());

  uint8_t result[32];  // SHA256 daje 32 bajta
  sha256.finalizeHMAC((const uint8_t*)key, strlen(key), result, sizeof(result));

  // Base64 URL kodiranje potpisa
  String signature = base64UrlEncode(String((char*)result));  // Ispravno konvertovanje bajtova u string

  return message + "." + signature;
}

void setup() {
  Serial.begin(115200);
  dht.begin(); // Inicijalizacija DHT senzora
  pinMode(PIRPIN, INPUT); // PIR senzor je ulazni pin
  WiFi.begin(ssid, password);

  // Povezivanje na WiFi
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Povezivanje...");
  }
  Serial.println("Povezano na WiFi!");

  // Pozivamo funkciju za kreiranje JWT tokena
  jwt = createJWT(secretKey, header, payload);  // Kreiramo JWT
  Serial.print("JWT: ");
  Serial.println(jwt); // Ispisivanje JWT tokena u serijskom monitoru
}

void loop(){
  sendDataToServer(jwt);
}

void sendDataToServer(String jwt) {
  // Čitanje podataka sa DHT senzora
  float temperature = dht.readTemperature(); // Čitanje temperature

  // Provera da li je očitavanje uspešno
  if (isnan(temperature)) {
    Serial.println("Greška u čitanju sa senzora!");
    return;  // Ako je očitavanje neuspešno, izlazi iz funkcije
  } else {
    Serial.print("Temperatura: ");
    Serial.print(temperature);
  }

  // Očitavanje stanja PIR senzora (detektuje pokret)
  int motionDetected = digitalRead(PIRPIN);
  if (motionDetected == HIGH) {
    Serial.println("Pokret detektovan!");
  } else {
    Serial.println("Nema pokreta.");
  }

  // Slanje podataka na server ako je WiFi povezan
  if (WiFi.status() == WL_CONNECTED) {
    WiFiClient client;
    HTTPClient http;

    // URL na koji šaljemo podatke
    String serverUrl = "http://192.168.241.232:7155/Device/ReciveDeviceResponse";
    http.begin(client, serverUrl); 
    http.addHeader("Content-Type", "application/json");
    http.addHeader("Authorization", "Bearer " + jwt);  // Koristimo ispravan JWT

    // Priprema JSON podataka za slanje
    String jsonData = "{\"deviceId\":\"" + String(deviceId) + 
                      "\", \"temperature\":" + String(temperature) + 
                      ", \"motionDetected\":" + String(motionDetected) + 
                      ", \"readingDateTime\":\"2024-11-19T22:59:58.369Z\"}";

    // Slanje HTTP POST zahteva sa JSON podacima
    int httpResponseCode = http.POST(jsonData);
    String response = "";
    response = http.getString();

    // Prikazivanje odgovora servera u serial monitoru
    Serial.print(httpResponseCode);
    //Serial.print("Response: ");
    //Serial.println(response);
    //Serial.println(jwt); 

    // Zatvaranje HTTP konekcije
    http.end();
  } else {
    Serial.println("Nema WiFi konekcije.");
  }

  delay(1000); // Čeka 1 sekundu pre sledećeg slanja
}
