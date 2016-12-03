// WebcamServer.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdio.h>
#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>

using namespace cv;
using namespace std;

#pragma comment(lib, "Ws2_32.lib")

#define DEFAULT_PORT "12345"
#define DEFAULT_BUFLEN 1024
#define MAX_THREADS 3


static int iResult;
int currentClients =  0;
WSADATA wsaData;
struct addrinfo *result = NULL, *ptr = NULL, hints;
SOCKET ListenSocket = INVALID_SOCKET;
SOCKET ClientSocket = INVALID_SOCKET;


Mat frame;
int imgSize;
vector<uchar> bufp;
DWORD WINAPI myVideoCapture(LPVOID lpParam);
HANDLE CameraThread;
HANDLE maint;
DWORD dwCameraThread;
HANDLE  hThreadArray[MAX_THREADS];
CRITICAL_SECTION CriticalSection;
HANDLE ghWriteEvent;



static class Helper {

	public:
		static void AcceptClient() {
		do {
			/*****this is only for TCP*/
			// Accept a client socket
			ClientSocket = accept(ListenSocket, NULL, NULL);
			if (ClientSocket == INVALID_SOCKET) {
				printf("accept failed: %d\n", WSAGetLastError());
				return;
			}
			else {
				printf("accept Success: %d\n", WSAGetLastError());
			}



			int imgSize = bufp.size();
			char* frameBytes = new char[imgSize];
			int iSendResult;

			// listen for client.
			int client_length = (int)sizeof(struct sockaddr_in);
			/* Receive bytes from client */
			int	bytes_received = recv(ClientSocket, frameBytes, imgSize, 0);
			//cout << frameBytes;
			if (bytes_received < 0)
			{
				fprintf(stderr, "Could not receive packet from %d.\n", ClientSocket);
			}
			int i = 0;
			do {//continuosly send new frame.
				// Send data here

				cout << "MAin is now waiting for camera thread \n";
				WaitForSingleObject(ghWriteEvent, INFINITE);
				cout << "main continue. \n";
				EnterCriticalSection(&CriticalSection);
				cout << "main entered the critical section \n";
				/*
				int x = bufp.size();
				char* tmpa = static_cast<char*>(static_cast<void*>(&x));
				cout << "Sending size \n";
				iSendResult = send(ClientSocket, tmpa, sizeof x, 0);
				if (iSendResult == SOCKET_ERROR) {
					printf("send failed: sent %d  %d\n", bufp.size(),WSAGetLastError());
				}
				else {
					printf("send Success: sent: %d\n",iSendResult);
				}
				*/
				
				char* tmpb = (char*)&bufp[0];
				cout << "Sending Data \n";
				iSendResult = send(ClientSocket, tmpb, bufp.size(), 0);
				if (iSendResult == SOCKET_ERROR) {
					printf("send failed: %d\n", WSAGetLastError());
				}
				else {
					printf("send Success: %d \n",iSendResult);
					printf("send dif: %d \n", iSendResult-bufp.size());
				}
				cout << "Frame: " << i++ << endl;
				
				LeaveCriticalSection(&CriticalSection);
				cout << "main left crit \n";
			} while (iSendResult > 0);
		}while (true);

	}

	static void InitializeListen() {
		ListenSocket = socket(result->ai_family, result->ai_socktype, result->ai_protocol);

		if (ListenSocket == INVALID_SOCKET) {
			printf("Error at socket(): %ld\n", WSAGetLastError());
			freeaddrinfo(result);
			WSACleanup();
			return;
		}
		else {
			printf("success at socket(): \n");
		}

		iResult = bind(ListenSocket, result->ai_addr, (int)result->ai_addrlen);
		if (iResult == SOCKET_ERROR) {
			printf("bind failed with error: %d\n", WSAGetLastError());
			freeaddrinfo(result);
			closesocket(ListenSocket);
			WSACleanup();
			return;
		} else {
			printf("bind success.\n");
		}

		freeaddrinfo(result);

		/******** this is only for TCP*/
		if (listen(ListenSocket, SOMAXCONN) == SOCKET_ERROR) {
			printf("Listen failed with error: %ld\n", WSAGetLastError());
			closesocket(ListenSocket);
			WSACleanup();
			return;
		}
		else {
			printf("Listen success\n");
		}
	

		printf("Listening for clients... \n");
	}

	static void InitializeWinsock() {
		// Initialize Winsock
		iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
		if (iResult != 0) {
			printf("WSAStartup failed: %d\n", iResult);
		}
		else {
			printf("WSAStartup success: %d\n", iResult);
		}
	}

	static void Resolveaddr() {
		ZeroMemory(&hints, sizeof(hints));
		hints.ai_family = AF_INET;
		hints.ai_socktype = SOCK_STREAM;
		hints.ai_protocol = IPPROTO_TCP;
		hints.ai_flags = AI_PASSIVE;

		// Resolve the local address and port to be used by the server
		iResult = getaddrinfo(NULL, DEFAULT_PORT, &hints, &result);
		if (iResult != 0) {
			printf("getaddrinfo failed: %d\n", iResult);
			WSACleanup();
		}
		else {
			printf("getaddrinfo success: %d\n", iResult);
		}
	}

	static void CloseClient(SOCKET ) {
		// shutdown the send half of the connection since no more data will be sent
		iResult = shutdown(ClientSocket, SD_SEND);
		if (iResult == SOCKET_ERROR) {
			printf("shutdown failed: %d\n", WSAGetLastError());
			closesocket(ClientSocket);
		}

		// cleanup
		closesocket(ClientSocket);
	}
};

int main() {
	
	InitializeCriticalSectionAndSpinCount(&CriticalSection,
		0x00000400);

	ghWriteEvent = CreateEvent(
		NULL,               // default security attributes
		TRUE,               // manual-reset event
		FALSE,              // initial state is nonsignaled
		TEXT("WriteEvent")  // object name
		);

	maint = GetCurrentThread();

	Helper::InitializeWinsock();

	Helper::Resolveaddr();

	Helper::InitializeListen();

	CameraThread = CreateThread(
		NULL,                   // default security attributes
		0,                      // use default stack size  
		myVideoCapture,       // thread function name
		NULL,          // argument to thread function 
		0,                      // use default creation flags 
		&dwCameraThread);   // returns the thread identifier 

	Helper::AcceptClient();


	int temp;
	cin >> temp;
	WSACleanup();
	return 0;
}

DWORD WINAPI myVideoCapture( LPVOID lpParam ) {

	VideoCapture cap(0);
	if (!cap.isOpened()) {
		cout << "Error opening video stream or file" << endl;
		return -1;
	}
	double dWidth = cap.get(CV_CAP_PROP_FRAME_WIDTH); //get the width of frames of the video
	double dHeight = cap.get(CV_CAP_PROP_FRAME_HEIGHT); //get the height of frames of the video
	cout << "Frame size : " << dWidth << " x " << dHeight << endl;

	vector<int> compression_params;
	compression_params.push_back(IMWRITE_JPEG_QUALITY);
	compression_params.push_back(75);

	for (;;) {
		EnterCriticalSection(&CriticalSection);
		ResetEvent(ghWriteEvent);
		//cout << "camera thread entered the critical section.\n";
		cap >> frame;
		bool bufsize = imencode(".jpeg", frame, bufp, compression_params);
		imgSize = bufp.size();
		if (!bufsize) {
			cout << "erorr encoding the file to jpeg." << endl;
		}
		imshow("Frame", frame);
		//cout << "Camera signalled main and is now going into the waiting state.\n";
		SetEvent(ghWriteEvent);
		LeaveCriticalSection(&CriticalSection);
		
		char c = (char)waitKey(11);
		if (c == 27) break;
	}

}

