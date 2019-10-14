void setup() {
  Serial.begin(9600);
}

unsigned long cur_millis = 0;
unsigned long pre_millis = 0;
double renew_xy_pos[][4] = { 0, };
float x = 0, y = 0, e = 0;
int f = 0, g = 0, cnt = 0;

float x_float = 0, y_float = 0, e_float = 0;
char f_char[10], g_char[5], x_char[10], y_char[10], e_char[10];
int g_int, f_int, j = 0, k = 0;

float atf(char buff[]);
byte leng;

char gcode[100];
char buf[10] = {0, };

void loop() {
  if (Serial.available()) {
    /*
        if (Serial.find("G") || Serial.find("X") || Serial.find("Y") || Serial.find("E") || Serial.find("F")) {

          leng = Serial.readBytesUntil('X', buf, 10);
          g = atoi(buf);

          leng = Serial.readBytesUntil('Y', buf, 10);
          x = atof(buf);

          leng = Serial.readBytesUntil('E', buf, 10);
          y = atof(buf);

          leng = Serial.readBytesUntil('F', buf, 10);
          e = atof(buf);

          leng = Serial.readBytesUntil('\0', buf, 10);
          if (atoi(buf) > 5)
            f = atoi(buf);
          else
            f = f;
    */

    //String gcode = Serial.readString();
    leng = Serial.readBytes(gcode, 50);
    /*
        Serial.println(leng);
        for (int i = 0; i < leng; i++) {
          Serial.print(gcode[i]);
        }
        Serial.println();
    */
    //Serial.println(leng);
    for (int i = 0; i < leng; i++) {

      if (gcode[i] == 'G') {
        j = i;
        while (gcode[j] != ' ') {
          j++;

          g_char[k] = gcode[j];
          k++;
        }
        k = 0;
        g_int = atoi(g_char);
        Serial.print("G = " + String(g_int));
      }
      if (gcode[i] == 'X') {
        j = i;
        while (gcode[j] != ' ')
        {
          j++;

          x_char[k] = gcode[j];
          k++;
        }
        k = 0;
        x_float = atof(x_char);
        Serial.print(", X = " + String(x_float));
      }
      if (gcode[i] == 'Y') {
        j = i;
        while (gcode[j] != ' ') {
          j++;

          y_char[k] = gcode[j];
          k++;
        }
        k = 0;
        y_float = atof(y_char);
        Serial.print(", Y = " + String(y_float));
      }
      if (gcode[i] == 'E') {
        j = i;
        while (gcode[j] != ' ') {
          j++;

          e_char[k] = gcode[j];
          k++;
        }
        k = 0;
        e_float = atof(e_char);
        Serial.print(", E = " + String(e_float));
      }
      if (gcode[i] == 'F') {
        j = i;
        while (gcode[j] != '\0') {
          j++;

          f_char[k] = gcode[j];
          k++;
        }
        k = 0;
        f_int = atoi(f_char);
        Serial.print(", F = " + String(f_int));
      }
    }
    /*
        Serial.print("G = " + String(g_char) + ", X = " + String(x_char) + ", Y = " + String(y_char));
        Serial.println(", E = " + String(e_char) + ", F = " + String(f_char) + ", CNT = " + String(cnt));
    */


    //cnt++;
    /*
      Serial.println(g_int);
      Serial.println(x_float);
      Serial.println(y_float);
      Serial.println(e_float);
      Serial.println(f_int);
    */
    Serial.println(", CNT = " + String(cnt));

    cnt++;
    j = 0; k = 0;
    //Serial.print("G = " + String(g) + ", X = " + String(x) + ", Y = " + String(y));
    //Serial.println(", E = " + String(e) + ", F = " + String(f) + ", CNT = " + String(cnt));
  }
}
