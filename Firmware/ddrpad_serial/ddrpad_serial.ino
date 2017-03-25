#define SERIAL_BUFFER_SIZE 16

const int NUM_PINS = 6;
char pins[] = {2,3,4,5,6,7,8};
int state = 1<<NUM_PINS;
int lastState = 0;

void setup() {
  Serial.begin(115200);
  
  for(int i = 0; i < NUM_PINS; i++){
    pinMode(pins[i], INPUT_PULLUP);
  }
}


void loop() {
  // put your main code here, to run repeatedly:
  for(int i = 0; i < NUM_PINS; i++){
    char v = digitalRead(pins[i]);
    state = bitWrite(state,i,v);
    //Serial.write(v?'1':'0');
  }

  //if(state!=lastState){
    Serial.println(state, BIN);
    Serial.flush();
    lastState = state;
  //}
  
  
}
