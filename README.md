# Openconfig Yang Browser


## Introduction

Welcome to the Openconfig Yang Browser, a Windows Presentation Foundation (WPF) application developed by Skyline Communications. This powerful tool enables users to navigate, explore, and interact with YANG models, making it easier than ever to manage and configure network devices that support the GNMI protocol using OpenConfig models.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
  - [Installation](#installation)
  - [Usage](#usage)

## Features

### 1. Files

The Openconfig Yang Browser allows users to manage YANG files efficiently. Here's what you can do with the "Files" feature:

- **Choose a Folder:** Select a folder to parse YANG files from. The application will automatically parse these files into objects.
- **File Listing:** View a list of all correctly parsed YANG files, making it easy to browse and manage them.
- **Import Validation:** Identify YANG files that are mentioned as imports in other YANG modules but do not exist in the chosen folder.

### 2. TreeView

The heart of the Openconfig Yang Browser is the TreeView feature, which provides a structured visualization of YANG models:

- **Tree Architecture:** Explore the structure of YANG modules as a treeview, with icons indicating the type of each node (groupings, containers, lists, and leafs).
- **Property Grid:** On selecting a node in the treeview, a property grid appears on the right, offering detailed information about the node.
- **GNMI Interaction:** Easily send requests for the values of specific nodes to network devices using the GNMI protocol. The response is displayed in a pop-up window.

### 3. Settings

Configuring the GNMI connection is crucial for seamless interaction with network devices. The "Settings" tab provides the necessary setup:

- **Connection Parameters:** Specify the IP address, port, username, and password required to connect to the network device.
- **Security:** Choose between HTTP and HTTPS, depending on the security configuration of your network device. Ensure the correct choice for successful connections.

## Getting Started

### Installation

1. Clone the repository to your local machine:

   ```bash
   git clone https://github.com/your-username/Openconfig-Yang-Browser.git
Open the project in Visual Studio or your preferred WPF development environment.

Build and run the application.

### Usage

### 1. Files:

- Click on the "Files" tab.
- Choose a folder containing your YANG files.
- View the list of correctly parsed files and imports.

### 2. TreeView:

- Click on the "TreeView" tab.
- Explore the YANG modules in a treeview format.
- Click on nodes to view details and send GNMI requests.

### 3. Settings:

- Click on the "Settings" tab.
- Configure the connection settings including IP, port, username, password, and security protocol (HTTP/HTTPS).
- Check "Enable subfolders" to enable parsing .yang files from subfolders contained in choosen folder
