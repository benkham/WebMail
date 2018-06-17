# WebMail

This app sends email to To, Cc, Bcc email address inputs using SendGrid and MailGun providers. If any of the registered provider fails, failover to the next available provider. 

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 1.7.3.

## SPA app

Update `apiUrl` value to `http://localhost:<PORT>/api` in `environment.ts` file.

Run `npm install` to install the packages.

Run `npm start` for a dev server. Navigate to `http://localhost:<PORT>/`. The app will automatically reload if you change any of the source files.

## API App

Update the settings value in `appsettings.json` file.

Run the SPA app before starting the API app from visual studio.

Swagger UI is accessible from `http://localhost:<PORT>/swagger`.

## Technologies

Front-end: Angular 5+, Back-end: ASP.Net Core 2.0

