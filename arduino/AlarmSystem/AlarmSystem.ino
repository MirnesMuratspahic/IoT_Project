#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <DHT.h>
#include <Crypto.h>
#include <SHA256.h>
#include <Base64.h>

#define DHTPIN D4
#define DHTTYPE DHT11
#define PIRPIN D2

DHT dht(DHTPIN, DHTTYPE);

const char* ssid = "UG99DB";
const char* password = "BD3C4877";
String deviceId = "0EC20056-FDC1-45CD-A10C-60C46571612F";
const char* header = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
String payload = "{\"sub\":\"" + deviceId + "\",\"iat\":1609459200,\"roles\":\"Device\"}";
const char* secretKey = "xCgmgpqPadKTaUgWF8j4YX3DZP7cgzYPSkG7GhJYbCXkroJVGMR3R6LFxRP1xwl5";
String jwt = "";

String base64UrlEncode(const String& input) {
  String encoded = base64::encode((const uint8_t*)input.c_str(), input.length());
  encoded.replace("+", "-");
  encoded.replace("/", "_");
  encoded.replace("=", "");
  return encoded;
}

String createJWT(const char* key, const char* header, const String& payload) {
  String encodedHeader = base64UrlEncode(header);
  String encodedPayload = base64UrlEncode(payload);
  String message = encodedHeader + "." + encodedPayload;

  SHA256 sha256;
  sha256.resetHMAC((const uint8_t*)key, strlen(key));
  sha256.update((const uint8_t*)message.c_str(), message.length());

  uint8_t result[32];
  sha256.finalizeHMAC((const uint8_t*)key, strlen(key), result, sizeof(result));

  String signature = base64UrlEncode(String((char*)result));

  return message + "." + signature;
}

void setup() {
  Serial.begin(115200);
  dht.begin();
  pinMode(PIRPIN, INPUT);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi...");
  }
  Serial.println("Connected to WiFi");

  jwt = createJWT(secretKey, header, payload);
  Serial.print("JWT: ");
  Serial.println(jwt);
}

void loop() {
  sendDataToServer(jwt);
}

void sendDataToServer(String jwt) {
  float temperature = dht.readTemperature();

  if (isnan(temperature)) {
    Serial.println("Error reading temperature");
    return;
  } else {
    Serial.print("Temperature: ");
    Serial.print(temperature);
  }

  int motionDetected = digitalRead(PIRPIN);
  if (motionDetected == HIGH) {
    Serial.println("Motion detected");
  } else {
    Serial.println("No motion");
  }

  if (WiFi.status() == WL_CONNECTED) {
    WiFiClient client;
    HTTPClient http;

    String serverUrl = "http://192.168.0.12:5031/Device/ReciveDeviceResponse";
    http.begin(client, serverUrl);
    http.addHeader("Content-Type", "application/json");
    http.addHeader("Authorization", "Bearer " + jwt);

    String jsonData = "{\"deviceId\":\"" + String(deviceId) +
                      "\", \"temperature\":" + String(temperature) +
                      ", \"motionDetected\":" + String(motionDetected) +
                      ", \"readingDateTime\":\"2024-11-19T22:59:58.369Z\"}";

    int httpResponseCode = http.POST(jsonData);
    String response = "";
    response = http.getString();

    Serial.print(httpResponseCode);

    http.end();
  } else {
    Serial.println("No WiFi connection");
  }

  delay(10000);
}
