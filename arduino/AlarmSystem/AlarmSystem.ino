#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <DHT.h>

// Definicija pina i tipa senzora
#define DHTPIN D3     // Pin na koji je povezan DATA pin DHT11
#define DHTTYPE DHT11 // Tip senzora (DHT11)
#define PIRPIN D2     // Pin na koji je povezan signalni pin PIR senzora

DHT dht(DHTPIN, DHTTYPE); // Kreiranje objekta senzora

const char* ssid = "UG99DB";      
const char* password = "BD3C4877";    
const char* deviceId = "D8BD6A6A-F3DB-45EE-34F4-08DD08BEAF44";  // Jedinstveni device ID

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
}

void loop() {
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
  int motionDetected = 0;
  motionDetected = digitalRead(PIRPIN);
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
    String serverUrl = "http://192.168.0.9:7155/api/Device/ReciveDeviceResponse";
    http.begin(client, serverUrl); // Početak HTTP konekcije
    http.addHeader("Content-Type", "application/json");

    // Priprema JSON podataka za slanje
    String jsonData = "{\"deviceId\":\"" + String(deviceId) + 
                      "\", \"temperature\":" + String(temperature) + 
                      ", \"motionDetected\":" + String(motionDetected) + 
                      ", \"readingDateTime\":\"2024-11-19T22:59:58.369Z\"}";

    // Slanje HTTP POST zahteva sa JSON podacima
    int httpResponseCode = http.POST(jsonData);
    String response = http.getString();

    // Prikazivanje odgovora servera u serial monitoru
    Serial.print("Response: ");
    Serial.println(response);

    // Zatvaranje HTTP konekcije
    http.end();
  } else {
    Serial.println("Nema WiFi konekcije.");
  }

  delay(1000); // Čeka 10 sekundi pre sledećeg slanja
}
