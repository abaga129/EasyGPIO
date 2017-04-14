# EasyGPIO
A simple client/server application to easily and remotely control a Raspberry Pi's GPIO pins.  Currently this is designed for models that use a 40-pin GPIO header but it can easily be adapted to other models.

![alt text](http://www.modernmetalproduction.com/wp-content/uploads/edd/2017/01/client-e1484251710962.png)

![alt text](http://www.modernmetalproduction.com/wp-content/uploads/2017/01/run.png)

If you would rather just use the pre-built binary, you can download it here - [EasyGPIO_client.zip](www.modernmetalproduction.com/free-downloads/EasyGPIO_client.zip)

## Building & Running

### Server
Currently the server is a simple python script. You will find it in /Server/.  All you need to do is put this script on your Raspberry Pi give it execution privledges, and run it using bash or python.

Example

`chmod +x EasyGPIO_server.py`

`./EasyGPIO_server.py` or  `python EasyGPIO_server.py`

That's all there is to it.  The server should find your local ip address and bind a socket to it on port 5005.  It will listen until a client connects.

### Client
The Client is a C# .net program. It provides a GUI with 3 modes for each pin on the Pi.

Building the client is fairly straightforward. Just open 'EasyGPIO.sln' using either Visual Studio or Mono-Develop and build it.  It has been built and tested on both Windows and Ubuntu.

#### Windows
Install .net framework (most windows installations already have it installed)
#### Linux
Install mono-complete `sudo apt-get install mono-complete`


## Data Model
The client and server communicate over a TCP socket. The client and server exchange data back and forth over a fixed interval that is defined in the server script.  By default this interval is every 100ms.  The reason for this is to save bandwidth.  The actual time between exchanges will vary depending on the network.

Each pin can be set to 1 of 3 modes.

+ Input: This mode sets the pin to input with a pull-down resistor. When the pin on the Pi detects an input, the server will send the appropriate data back to the client and the control area for that pin will change to a bright red color.
+ Output High: This mode simply turns the pin on so that it is putting out 3.3v.
+ Output Low: This mode is basically the off state of the pin.

### The basic cycle for the client and server is as follows.
1. The client checks all of the panels for user input and constructs a string containing the mode for each pin.
2. The client sends the string over an asynchronous socket and then waits to recieve data from the server.
3. The server recieves the string from the client.
4. The server walks through each character of the string and sets each output if neccesary and reads each input designated by the recieved string.  Simultaneously the server constructs a new string to show possible changes in pins set to input.
5. The server sends the newly constructed string to the client and waits to recieve data from the client.
6. The client reads the string recieved from the server and sets any input panels red if they were set to on from the server.
7. This process repeats until the client disconnects.

### Format of the strings being sent
The strings transmited through the socket are 160 characters long which means 160bytes of data plus headers for each packet. The string can be divided into 40 separate strings of 4 characters; each representing a pin on the Pi. We will call these substrings.
The first two characters of each substring is the pin number.  The third character represents Input or output. The fourth character represents the level of the pin.

|Index|Meaning|        Possible Value                   |
|:---:|:-----:|:---------------------------------------:|
| 0-1 | Pin # | 01 - 40 (must be two bytes)             |
|  2  | Mode  |"I"=input, "O"=output, "X"=Off           |
|  3  | Level |"H"=high, "L"=low (set to "L" by default)|

Example string

`"01XL02XL03OH04OL05IL06IL.......39XL40OH"`

### Things to Consider
+ There are several pins on the header that cannot be used: grounds, 5v rails, etc. These pins are automatically set to the X mode and there is no option to set them in the GUI but they still have a panel to make things more organized.
+ Some pins have more complex functions such as PWM which is not yet implemented. This will require a more complex data model but I would like to see this implemented in the future.
