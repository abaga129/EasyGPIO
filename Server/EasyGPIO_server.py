#!/usr/bin/python
###################################################################
#EasyGPIO Server
#Version 1.0 beta
#Written by: Ethan Reker
#For technical support contact Ethan at ctrecordingslive@gmail.com
###################################################################

import socket
import RPi.GPIO as GPIO
import time
import os
from datetime import datetime

print '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'
print 'EasyGPIO Server - Version 1.0 beta'
print 'Written by: Ethan Reker - www.modernmetalproduction.com'
print 'For technical support, email Ethan at ctrecordingslive@gmail.com'
print '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'
print '\nLogging data to EasyPIOLog.txt..'

TCP_PORT = 5005
BUFFER_SIZE = 160
PREV_DATA = ''
PREV_SEND = ''

log = open('EasyPIOLog.txt', 'w')
log.write('EasyPIOLog.txt\n\n')


ip = ""
f = os.popen('ifconfig eth0 | grep "inet\ addr" | cut -d: -f2 | cut -d " " -f1')
ip = f.read()
if ip == '':
    f = os.popen('ifconfig wlan0 | grep "inet\ addr" | cut -d: -f2 | cut -d " " -f1')
    ip = f.read()
    if ip == '':
        print 'No Local IP Address Found Please Make Sure You Are Connected To The Local Network.'
        log.write('No Local IP Address Found Please Make Sure You Are Connected To The Local Network.\n')
    else:
        print 'Local IP on wlan0: ', ip
        log.write('Local IP on wlan0: ' +  ip + '\n')
else:
    print 'Local IP on eth0: ', ip
    log.write('Local IP on eth0: ' + ip + '\n')

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((ip, TCP_PORT))
s.listen(1)


while 1:
    GPIO.setmode(GPIO.BOARD)
    GPIO.setwarnings(False)

    print 'Waiting for connection..'
    log.write('Waiting for connection..\n')
    log.close()

    conn, addr = s.accept()
    conn.send('Handshake')

    log = open('EasyPIOLog.txt', 'a')
    print 'Connection address:', addr, '\n'
    log.write('Connection address: ' + str(addr) + '\n\n')

    while 1:
        data = conn.recv(BUFFER_SIZE)
        if not data: break
        
        #CHECK THAT MESSAGE IS 160B
        if len(str(data)) != BUFFER_SIZE: break
        sendData = ""
        for i in range(0,40):
            j = i * 4
            pinNo = int(data[j:j+2])
            mode = data[j+2]
            level = data[j+3]

            if pinNo < 10:
                sendData += '0'
            sendData += str(pinNo) + mode
            
            if mode == 'O':
                sendData += level
                GPIO.setup(pinNo, GPIO.OUT)
                if level == 'H':
                    GPIO.output(pinNo, GPIO.HIGH)
                else:
                    GPIO.output(pinNo, GPIO.LOW)
            elif mode == 'I':
                GPIO.setup(pinNo, GPIO.IN)
                if GPIO.input(pinNo):
                    sendData += 'H'
                else:
                    sendData += 'L'
            else:
                sendData += 'L'
        if PREV_DATA != data or PREV_SEND != sendData:
            print datetime.now()
            print 'Received Data: ', data
            print 'Data Sent Back: ', sendData, '\n'
            log.write(str(datetime.now()) + '\nReceived Data: ' + data + '\nData Sent Back: ' + sendData + '\n\n')
        conn.send(sendData)
        PREV_DATA = data
        PREV_SEND = sendData                  
        time.sleep(0.1)
    GPIO.cleanup()    
    conn.close()
        
#sample message "01OH02IL03XL05OH05IL06XL07OH08IL09XL10OH11IL12XL13OH14IL15XL16OH17IL18XL01OH02IL03XL05OH05IL06XL07OH08IL09XL10OH11IL12XL13OH14IL15XL16OH17IL18XL
    
