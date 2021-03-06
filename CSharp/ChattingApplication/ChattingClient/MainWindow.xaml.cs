﻿using ChattingInterfaces;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace ChattingClient {

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public static IChattingService Server;
		private static DuplexChannelFactory<IChattingService> channelFactory;

		public MainWindow() {
			InitializeComponent();
			channelFactory = new DuplexChannelFactory<IChattingService>(new ClientCallback(), "ChattingServiceEndPoint");
			Server = channelFactory.CreateChannel();
		}

		private void LoginButtonClick(object sender, RoutedEventArgs e) {
			LoginSubmit();
		}

		private void LoginKeyUp(object sender, KeyEventArgs e) {
			if (e.Key == Key.Enter) LoginSubmit();
		}

		private void LoginSubmit() {
			int returnValue = Server.Login(UserNameTextBox.Text);
			if (returnValue == 1) {
				TakeMessage("You are already logged in.", "System");
			} else if (returnValue == 0) {
				TakeMessage("You are logged in", "System");
				WelcomeLabel.Content = "Welcome " + UserNameTextBox.Text + "!";
				UserNameTextBox.IsEnabled = false;
				LoginButton.IsEnabled = false;

				LoadUserList(Server.GetCurrentUsers());
			}
		}

		private void SendButtonClick(object sender, RoutedEventArgs e) {
			InputSend();
		}

		private void InputKeyUp(object sender, KeyEventArgs e) {
			if (e.Key == Key.Enter) InputSend();
		}

		private void InputSend() {
			if (InputTextBox.Text.Length > 0) {
				Server.SendMessageToAll(InputTextBox.Text, UserNameTextBox.Text);
				TakeMessage(InputTextBox.Text, "You");
				InputTextBox.Text = "";
			}
		}

		public void TakeMessage(string message, string userName) {
			TextDisplayTextBox.Text += userName + ": " + message + "\n";
			TextDisplayTextBox.ScrollToEnd();
		}

		private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e) {
			Server.Logout();
		}

		public void AddUserToList(string userName) {
			if (!UsersListBox.Items.Contains(userName)) {
				UsersListBox.Items.Add(userName);
			}
		}

		public void RemoveUserFromList(string userName) {
			if (UsersListBox.Items.Contains(userName)) {
				UsersListBox.Items.Remove(userName);
			}
		}

		private void LoadUserList(List<string> users) {
			foreach (var user in users) {
				AddUserToList(user);
			}
		}
	}
}