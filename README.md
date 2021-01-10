# eID_Signature
 
In order to finish my bachelor program **Applied Computer Sience - Software Development** at **PXL Hogeschool Hasselt**, I was tasked to develop a web application for the school itself and to write a bachelor paper. In this paper I examined the legal and technical aspects to use the **Belgian Electronic ID** within an application.

For this research I also created a small **WPF Application** to investigate how a pdf-file could be **digitally signed** using the **eID**. This application can be found in this directory and can only be used with an eID from Belgium.

## Project
The application uses the **bepkcs11.dll module** to create a connection between the application and the eID. This library (in the wrappers folder) is referenced to in the application.

- WPF-project: .NET Framework 4.7.2
- Business-project: .NET Standard 2.0

The application can simply be runned within Visual Studio 2019.

## License
* PXL Switch2IT - Maarten Warson (academiejaar 2020-2021)
