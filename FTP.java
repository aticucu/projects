import java.lang.System;
import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.SocketTimeoutException;
import java.net.UnknownHostException;
import java.lang.String;


//
// This is an implementation of a simplified version of a command 
// line ftp client. The program takes no arguments.
//

// GY Merge
// Junoh Test
// Index fff


public class CSftp
{
	static final int MAX_LEN = 255;
	private static Socket socket;
	private static Socket dataSocket;
	private static String cmd;
	private static String parameter1;
	private static String parameter2;
	private static BufferedWriter writer;
	private static BufferedWriter dataWriter;
	private static BufferedReader reader;
	private static BufferedReader dataReader;
	private static String entireCmd;
	private static String[] cmdnpara;
	private static String dataIp;
	private static int dataPort;
	private static String data;
	private static int port;
	private static String servername;

	public static void main(String [] args)
	{

		String password;

		try {
			for (int len = 1; len > 0;) {
				byte cmdString[] = new byte[MAX_LEN];
				System.out.print("csftp> ");
				len = System.in.read(cmdString);
				if (len <= 0) 
					break;
				// Start processing the command here.

				// Refactored to getCommand
				getCommand(cmdString);

				/* When command is open, make connection to the 
				 * given domain name/IP address. If port is given,
				 * connect to the specific port.
				 */
				if(cmd.equals("open")) {
					if(socket != null) {
						socket.close();
					}
					if(cmdnpara.length == 2) {
						socket = new Socket();
						try {
							socket.connect(new InetSocketAddress(parameter1, 21), 30000);
						} catch (SocketTimeoutException e) {
							System.out.println("920 Control connection to " +
									parameter1 + " on port 21 failed to open");
						} catch (IOException e) {
							System.out.println("925 Control connection I/O error, closing control connection.");
						}
						System.out.println("Connected to " + parameter1 + ".");
					} else if(cmdnpara.length == 3) {
						try { 
							port = Integer.parseInt(cmdnpara[2].trim());
						} catch (NumberFormatException e) { 
							System.out.println("902 Invalid Argument");
						}
						socket = new Socket();
						try {
							socket.connect(new InetSocketAddress(parameter1, port), 30000);
						} catch (SocketTimeoutException e) {
							System.out.println("920 Control connection to " +
									parameter1 + " on port " + port + " failed to open");
						} catch (IOException e) {
							System.out.println("925 Control connection I/O error, closing control connection.");
						}
						System.out.println("Connected to " + parameter1 + ".");
					} else {
						System.out.println("901 Incorrect number of arguments");
					}

					writer = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream()));
					reader = new BufferedReader(new InputStreamReader(socket.getInputStream()));

					readLine();
				}

				/*  
				 * When command is user, send username to the server
				 * and receive the message requesting password.
				 */
				else if(cmd.equals("user")) {
					if (socket == null || socket.isClosed()) {
						System.out.println("903 Supplied command not expected at this time");
					}
					else if(cmdnpara.length != 2) {
						System.out.println("901 Incorrect number of arguments");
					} else {
						sendLine(entireCmd);
						readLine();

						// Get password
						byte pwdString[] = new byte[MAX_LEN];
						System.out.print("Password> ");
						System.in.read(pwdString);
						password = new String(pwdString, "UTF-8").trim();

						String passcmd = "pass " + password;
						sendLine(passcmd);
						readLine();
					}
				}

				else if(cmd.equals("quit")) {
					if (socket == null || socket.isClosed()) {
						return;
					} else
						close();
					return;
				}

				else if(cmd.equals("close")) {
					if (socket == null) {
						System.out.println("903 Supplied command not expected at this time");
					} else
						close();
				}

				else if(cmd.equals("get")) {
					String fileName = parameter1;
					try {
						FileOutputStream fileStream = new FileOutputStream(fileName);
						BufferedOutputStream dataOut = new BufferedOutputStream(fileStream);
						if (socket == null || socket.isClosed()) {
							System.out.println("903 Supplied command not expected at this time");
						} else if(cmdnpara.length != 3) {
							System.out.println("901 Incorrect number of arguments");
						} else {
							sendLine("PASV");
							String line = reader.readLine();
							if(line.startsWith("530")) {
								System.out.println("903 Supplied command not expected at this time");
							} else
								try{ 
									dataIp = getIpAddress(line);
									dataPort = getDataPort(line);
									sendLine("RETR " + parameter2);
									dataSocket = new Socket(dataIp, dataPort);
									readLine();
									InputStream in = dataSocket.getInputStream();
									BufferedInputStream dataIn = new BufferedInputStream(in);
									int bufferSize = 4096;
									int i = 0;
									byte[] inputBuffer = new byte[bufferSize];
									while ((i = dataIn.read(inputBuffer)) != -1)
									{
										dataOut.write(inputBuffer, 0, i);
									}
									dataOut.flush();
									dataOut.close();
									dataIn.close();

									readLine();
									dataSocket.close();

								} catch (SocketTimeoutException e) {
									System.out.println("930 Data transfer to " +
											servername + " on port " + dataPort + " failed to open");
								} catch (FileNotFoundException e) {
									System.out.println("910 Access to local file " + fileName + " denied.");
								} catch (IOException e) {
									System.out.println("935 Data transfer connection I/O error, closing data connection.");
								} catch (Exception e) {
									System.out.println("999 Processing error.");
								}
						}
					} catch (FileNotFoundException e) {
						System.out.println("910 Access to local file " + fileName + " denied.");
					} 

				}
				else if(cmd.equals("put")) {
					String fileName = parameter1;
					try {
						FileInputStream fileStream = new FileInputStream(fileName);
						BufferedInputStream dataIn = new BufferedInputStream(fileStream);
						if (socket == null || socket.isClosed()) {
							System.out.println("903 Supplied command not expected at this time");
						} else if(cmdnpara.length != 3) {
							System.out.println("901 Incorrect number of arguments");
						} else {
							sendLine("PASV");
							String info = reader.readLine();
							if(info.startsWith("530")) {
								System.out.println("903 Supplied command not expected at this time");
							} else
								try{
									dataIp = getIpAddress(info);
									dataPort = getDataPort(info);
									sendLine("STOR " + parameter2);
									dataSocket = new Socket();
									dataSocket.connect(new InetSocketAddress(dataIp, dataPort), 30000);

									OutputStream out = dataSocket.getOutputStream();
									BufferedOutputStream dataOut = new BufferedOutputStream(out);
									int bufferSize = 4096;
									int i = 0;
									byte[] outputBuffer = new byte[bufferSize];
									while ((i = dataIn.read(outputBuffer)) != -1)
									{
										dataOut.write(outputBuffer, 0, i);
									}
									dataOut.flush();
									dataOut.close();
									dataIn.close();

									readLine();
									dataSocket.close();
								} catch (SocketTimeoutException e) {
									System.out.println("930 Data transfer to " +
											servername + " on port " + dataPort + " failed to open");
								} catch (IOException e) {
									System.out.println("935 Data transfer connection I/O error, closing data connection.");
								} catch (Exception e) {
									System.out.println("999 Processing error.");
								}
						}
					}
					catch (FileNotFoundException e) {
						System.out.println("910 Access to local file " + fileName + " denied.");
					} 
				}
				else if(cmd.equals("cd")) {
					if(cmdnpara.length != 2) {
						System.out.println("901 Incorrect number of arguments");
					} else
						cd();
				}
				else if(cmd.equals("dir")) {
					if(cmdnpara.length != 1) {
						System.out.println("901 Incorrect number of arguments");
					}else
						dir();
				} else {
					System.out.println("900 Invalid command.");
				}

			}
		} catch (IOException exception) {
			System.err.println("998 Input error while reading commands, terminating.");
		}

	}

	private static void cd() throws IOException {
		sendLine("CWD " + parameter1);
		readLine();
	}

	private static void dir() throws IOException, UnknownHostException {
		sendLine("PASV");
		String info = reader.readLine();
		if(info.startsWith("530")) {
			System.out.println("903 Supplied command not expected at this time");
		} else try{
			sendLine("LIST");
			dataIp = getIpAddress(info);
			dataPort = getDataPort(info);
			dataSocket = new Socket(dataIp, dataPort);
			dataWriter = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream()));
			dataReader = new BufferedReader(new InputStreamReader(dataSocket.getInputStream()));
			readLine();
			readDataLine();
			readLine();
		} catch (SocketTimeoutException e) {
			System.out.println("930 Data transfer to " +
					servername + " on port " + dataPort + " failed to open");
		} catch (IOException e) {
			System.out.println("999 Processing error.");
		} catch (Exception e) {
			System.out.println("999 Processing error.");
		}
	}

	private static String getIpAddress(String line) {
		int beginning = line.indexOf('(');
		int end = line.indexOf(')');
		String givenIp = line.substring(beginning + 1, end);
		int ip1 = givenIp.indexOf(',');
		int ip2 = givenIp.indexOf(',', ip1 + 1);
		int ip3 = givenIp.indexOf(',', ip2 + 1);
		int ip4 = givenIp.indexOf(',', ip3 + 1);
		int ip5 = givenIp.indexOf(',', ip4 + 1);

		String ipAddress = givenIp.substring(0, ip1) + "."
				+ givenIp.substring(ip1 + 1, ip2) + "."
				+ givenIp.substring(ip2 + 1, ip3) + "."
				+ givenIp.substring(ip3 + 1, ip4);

		System.out.println(ipAddress);
		return ipAddress;
	}


	private static int getDataPort(String line) {
		int beginning = line.indexOf('(');
		int end = line.indexOf(')');
		String givenIp = line.substring(beginning + 1, end);
		int ip1 = givenIp.indexOf(',');
		int ip2 = givenIp.indexOf(',', ip1 + 1);
		int ip3 = givenIp.indexOf(',', ip2 + 1);
		int ip4 = givenIp.indexOf(',', ip3 + 1);
		int ip5 = givenIp.indexOf(',', ip4 + 1);

		int upper = Integer.parseInt(givenIp.substring(ip4 + 1, ip5));
		int lower = Integer.parseInt(givenIp.substring(ip5 + 1));

		int port = upper * 256 + lower;
		System.out.println(port);
		return port;
	}

	// Close the socket
	private static void close() throws IOException {
		sendLine("QUIT");
		readLine();
		socket.close();
	}

	// Send a line to the server
	private static void sendLine(String line) throws IOException {
		if (line == "QUIT") {
			writer.write(line + "\r\n");
			writer.flush();
		}else if (socket == null || socket.isClosed()) {
			throw new IOException("FTP is not connected.");
		}
		try {
			writer.write(line + "\r\n");
			writer.flush();
		} catch (IOException e) {
			socket = null;
			System.out.println("999 Processing error. Unknown Host!");
		}
	}

	private static void sendDataLine(String line) throws IOException {
		if (dataSocket == null) {
			throw new IOException("CSFTP is not connected.");
		}
		try {
			dataWriter.write(line + "\r\n");
			dataWriter.flush();
		} catch (IOException e) {
			dataSocket = null;
			throw e;
		}
	}

	// Read line from the server and display it to the user
	private static void readLine() throws IOException {
		do {
			String line = reader.readLine();
			if (line.charAt(0) == 4) {
				System.out.println("925 Control connection I/O error, closing control connection");
				socket.close();
			}
			else
				System.out.println("Received: " + line);
		} while (reader.ready());
	}

	private static void readDataLine() throws IOException {
		do {
			String line = dataReader.readLine();
			if (line.charAt(0) == 4) {
				System.out.println("925 Control connection I/O error, closing control connection");
				socket.close();
			} else
				System.out.println("Received: " + line);
		} while (dataReader.ready());
	}

	// Get user input command and split it into command and parameters
	private static void getCommand(byte[] cmdString) throws IOException {
		entireCmd = new String(cmdString, "UTF-8");
		cmdnpara = entireCmd.split(" ");
		cmd = cmdnpara[0].trim();
		if(cmdnpara.length == 2) {
			parameter1 = cmdnpara[1].trim();
		}
		if(cmdnpara.length == 3) {
			parameter1 = cmdnpara[1].trim();
			parameter2 = cmdnpara[2].trim();
		}
	}
}
