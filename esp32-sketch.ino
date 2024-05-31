#include <WiFi.h>
#include <Wire.h>
#include <ArduinoWebsockets.h>
#include <NTPClient.h>
#include <WiFiUdp.h>

using namespace websockets;

//WiFi
const char* ssid = ""; 
const char* password = "";

//sensor
#define address 0x40
uint8_t buf[4] = {0};
uint16_t data, data1;
float temp;
float hum;

WiFiUDP ntpUDP;
NTPClient timeClient(ntpUDP, "pool.ntp.org");

//WebSocket
WebsocketsClient client;
const char* websockets_server_host = ""; 
const uint16_t websockets_server_port = ;

void setup() {
  Serial.begin(9600);
  Wire.begin();

  WiFi.begin(ssid, password);
  Serial.print("Conectando a WiFi...");
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("Conectado a WiFi");

  timeClient.begin();
  timeClient.setTimeOffset(3600);

  client.onEvent([&](WebsocketsEvent event, String data) {
    if (event == WebsocketsEvent::ConnectionOpened) {
      Serial.println("Conexión WebSocket abierta");
    } else if (event == WebsocketsEvent::ConnectionClosed) {
      Serial.println("Conexión WebSocket cerrada");
    
      delay(30000);
      Serial.println("Intentando reconectar al servidor WebSocket...");
      connectToWebSocket();
    } else if (event == WebsocketsEvent::GotPing) {
      Serial.println("Ping recibido");
    } else if (event == WebsocketsEvent::GotPong) {
      Serial.println("Pong recibido");
    }
  });

  connectToWebSocket();
}

void loop() {
  readReg(0x00, buf, 4);
  data = buf[0] << 8 | buf[1];
  data1 = buf[2] << 8 | buf[3];
  temp = ((float)data * 165 / 65535.0) - 40.0;
  hum =  ((float)data1 / 65535.0) * 100;

  timeClient.update();
  String formattedDate = getFormattedDateTime();

  String jsonData = "{\"idRecord\":3,\"idPlant\":1,\"dateTime\":\"" + formattedDate + "\",\"temperature\":" + String(temp, 2) + ",\"humidity\":" + String(hum, 2) + "}";
  Serial.println("Enviando datos: " + jsonData);

  
  client.send(jsonData);

  
  client.poll();

  delay(10000); 
}

uint8_t readReg(uint8_t reg, const void* pBuf, size_t size) {
  if (pBuf == NULL) {
    Serial.println("pBuf ERROR!! : null pointer");
  }
  uint8_t* _pBuf = (uint8_t*)pBuf;
  Wire.beginTransmission(address);
  Wire.write(&reg, 1);
  if (Wire.endTransmission() != 0) {
    return 0;
  }
  delay(20);
  Wire.requestFrom(address, (uint8_t)size);
  for (uint16_t i = 0; i < size; i++) {
    _pBuf[i] = Wire.read();
  }
  return size;
}

String getFormattedDateTime() {
  
  time_t rawtime = timeClient.getEpochTime();
  struct tm* timeinfo = gmtime(&rawtime);
  
  char buffer[25];
  snprintf(buffer, sizeof(buffer), "%04d-%02d-%02dT%02d:%02d:%02d", 
           timeinfo->tm_year + 1900, 
           timeinfo->tm_mon + 1, 
           timeinfo->tm_mday, 
           timeinfo->tm_hour, 
           timeinfo->tm_min, 
           timeinfo->tm_sec);
  
  return String(buffer);
}

void connectToWebSocket() {
  Serial.println("Conectando al servidor WebSocket...");
  client.connect(websockets_server_host, websockets_server_port, "/");
}
