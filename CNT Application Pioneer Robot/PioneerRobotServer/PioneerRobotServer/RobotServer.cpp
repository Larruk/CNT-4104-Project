#include "include/Aria.h"
#include <iostream>
#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include <vector>
#include <sstream>

#pragma comment (lib, "Ws2_32.lib")

#undef UNICODE

#define WIN32_LEAN_AND_MEAN
#define DEFAULT_BUFLEN 512
#define DEFAULT_PORT "12346"

using namespace std;

vector<string> split(const string &s, char delim);
void Drive(int speed);
void Rotate(int speed);
void Stop();
void Dance(int speed);

ArRobot robot;

// Change this to false in release builds
bool debugBuild = false;

int COMMAND_DURATION = 1000;

int main(int argc, char** argv)
{
	Aria::init();
	ArArgumentParser parser(&argc, argv);
	parser.loadDefaultArguments();
	
	// ArRobotConnector connects to the robot, get some initial data from it such as type and name,
	// and then loads parameter files for this robot.
	ArRobotConnector robotConnector(&parser, &robot);
	if (!robotConnector.connectRobot() && !debugBuild)
	{
		ArLog::log(ArLog::Terse, "Could not connect to the robot.");
		if (parser.checkHelpAndWarnUnparsed())
		{
			Aria::logOptions();
			Aria::exit(1);
			return 1;
		}
	}

	ArLog::log(ArLog::Normal, "Connected.");
	robot.setConnectionTimeoutTime(0);
	robot.enableMotors();

	// Start the robot processing cycle running in the background.
	// True parameter means that if the connection is lost, then the 
	// run loop ends.
	robot.runAsync(true);

	// CORE LOGIC
	WSADATA wsaData;
	int iResult;

	SOCKET ListenSocket = INVALID_SOCKET;
	SOCKET ClientSocket = INVALID_SOCKET;

	struct addrinfo *result = NULL;
	struct addrinfo hints;

	int iSendResult;
	char recvbuf[DEFAULT_BUFLEN];
	int recvbuflen = DEFAULT_BUFLEN;

	// Initialize Winsock
	iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if (iResult != 0) {
		printf("WSAStartup failed with error: %d\n", iResult);
		return 1;
	}

	ZeroMemory(&hints, sizeof(hints));
	hints.ai_family = AF_INET;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;
	hints.ai_flags = AI_PASSIVE;

	// Resolve the server address and port
	iResult = getaddrinfo(NULL, DEFAULT_PORT, &hints, &result);
	if (iResult != 0) {
		printf("getaddrinfo failed with error: %d\n", iResult);
		WSACleanup();
		return 1;
	}

	// Create a SOCKET for connecting to server
	ListenSocket = socket(result->ai_family, result->ai_socktype, result->ai_protocol);
	if (ListenSocket == INVALID_SOCKET) {
		printf("socket failed with error: %ld\n", WSAGetLastError());
		freeaddrinfo(result);
		WSACleanup();
		return 1;
	}

	// Setup the TCP listening socket
	iResult = ::bind(ListenSocket, result->ai_addr, (int)result->ai_addrlen);
	if (iResult == SOCKET_ERROR) {
		printf("bind failed with error: %d\n", WSAGetLastError());
		freeaddrinfo(result);
		closesocket(ListenSocket);
		WSACleanup();
		return 1;
	}

	freeaddrinfo(result);

	iResult = listen(ListenSocket, SOMAXCONN);
	if (iResult == SOCKET_ERROR) {
		printf("listen failed with error: %d\n", WSAGetLastError());
		closesocket(ListenSocket);
		WSACleanup();
		return 1;
	}

	double v = robot.getRealBatteryVoltageNow();
	printf("Voltage: %f\n", v);

	// Accept a client socket
	printf("Listening on port 8080..\n");
	ClientSocket = accept(ListenSocket, NULL, NULL);
	if (ClientSocket == INVALID_SOCKET) {
		printf("accept failed with error: %d\n", WSAGetLastError());
		closesocket(ListenSocket);
		WSACleanup();
		return 1;
	}

	// No longer need server socket
	closesocket(ListenSocket);

	printf("Client connection opened...\n");
	int msgLength;

	// CORE FUNCTION OF SERVER
	// Receive until the peer shuts down the connection
	do {
		v = robot.getRealBatteryVoltageNow();
		printf("Battery Level: %f\n", v);
		// Get the size of the message received
		msgLength = recv(ClientSocket, recvbuf, recvbuflen, 0);
		if (msgLength > 0) {
			vector<char> charResult;
			for (int i = 0; i < msgLength; i++) {
				charResult.push_back(recvbuf[i]);
			}

			// Result!
			string result(charResult.begin(), charResult.end());
			vector<string> commandVector = split(result, '|');

			if (commandVector.size() > 1) {
				cout << commandVector.at(0) << " : " << commandVector.at(1) << endl;

				if (commandVector.at(0) == "forward") {
					Drive(stoi(commandVector.at(1)));
				}
				else if (commandVector.at(0) == "backward") {
					Drive(-1 * stoi(commandVector.at(1)));
				}
				else if (commandVector.at(0) == "left") {
					Rotate(stoi(commandVector.at(1)));
				}
				else if (commandVector.at(0) == "right") {
					Rotate(-1 * stoi(commandVector.at(1)));
				}
				else if (commandVector.at(0) == "dance"){
					Dance(stoi(commandVector.at(1)));
				}
				ArUtil::sleep(COMMAND_DURATION);
				Stop();
			}
			else {
				printf("Please use the following format for commands\ncommand|speed\n");
			}
		}
		else if (msgLength == 0)
			printf("\t\tConnection closing...\n");
		else {
			printf("recv failed with error: %d\n", WSAGetLastError());
			closesocket(ClientSocket);
			WSACleanup();
			return 1;
		}
	} while (msgLength > 0);

	// shutdown the connection since we're done
	iResult = shutdown(ClientSocket, SD_SEND);
	if (iResult == SOCKET_ERROR) {
		printf("shutdown failed with error: %d\n", WSAGetLastError());
		closesocket(ClientSocket);
		WSACleanup();
		return 1;
	}

	// cleanup
	closesocket(ClientSocket);
	WSACleanup();

	robot.stopRunning();

	// wait for the thread to stop
	robot.waitForRunExit();

	// exit
	ArLog::log(ArLog::Normal, "Exiting.");
	Aria::exit(0);
	return 0;
}

void Drive(int speed) {
	printf("Moving at a speed of %i m/s\n", speed);
	if (debugBuild) {
		return;
	}

	robot.lock();
	robot.setVel(speed);
	robot.unlock();
	return;
}

void Rotate(int speed) {
	printf("Rotating at a speed of %i m/s\n", speed);
	if (debugBuild) {
		return;
	}
	
	robot.lock();
	robot.setRotVel(speed);
	robot.unlock();
	return;
}

void Dance(int speed) {
	printf("Dancing!!!\n");
	if (debugBuild) {
		return;
	}

	Rotate(speed);
	ArUtil::sleep(COMMAND_DURATION);

	for (int i = 0; i < 5; i++) {
		Rotate(speed);
		ArUtil::sleep(COMMAND_DURATION);
		speed *= -1;
	}

	speed *= -1;
	Rotate(speed);
	return;
}

void Stop() {
	printf("Stopping...\n");
	if (debugBuild) {
		return;
	}

	robot.lock();
	robot.stop();
	robot.unlock();
	return;
}

vector<string> split(const string &s, char delim) {
	stringstream ss(s);
	string item;
	vector<string> tokens;
	while (getline(ss, item, delim)) {
		tokens.push_back(item);
	}
	return tokens;
}