<img src="https://github.com/DDeveloperBG/StudentsHelper/blob/master/src/Web/StudentsHelper.Web/wwwroot/favIcon/android-chrome-256x256.png?raw=true" width="50" height="50px" alt="SH">

# StudentsHelper
Web Application to help students with their homework, lessons, etc. With immediate consultations about the topic made with a professional teacher on the subject.
**[Website URL](https://studentshelper.azurewebsites.net/)**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Application flow

 Student
* Registers
* Adds money to his balance
* Looks for a teacher at the specific topic
* Books consultation at teacher details
* When start time comes he joins the consultation

Teacher
* Registers
* Adds information to Stripe about his bank account so that he can receive his monthly salary
* Waits to be approved by Admin so that he can be listed to Students
* When a student books him he can see the consultation on the *Consultations* page
* When consultation start time comes he can join

Admin
* Confirms teachers

## Roles

* Administrator
* Teacher
* Student

## Getting Started

Site guest (**visitor**) 
* Can only visit shared pages (*Home*, *All Teachers*, *Contact Us*, *Register*, *Login*).
* Can send e-mail to the site owner.

**Administrator** - seeded automatically
* Manage teachers.
* Manage Hangfire dashboard.

**Teacher**
* Can visit shared pages and (*Teacher Details*, *Consultations*, *Video Chat*).
* Can enter consultations.

**Student**
* Can visit shared pages and (*Teacher Details*, *Book Consultation*, *Consultations*, *Show Add Review*, *Video Chat*).
* Can add money to his balance.
* Can book consultations.
* Can enter consultations.

## Background processes

**Hangfire** has two registered jobs
* Every consultation has an event for the end time and taxes the student based on the meeting duration.
* Every month to collect monthly commissions from teachers monthly salaries.

## Technologies Used

* C#
* ASP.Net 6
* Entity Framework 6
* MS SQL Server
* Bootstrap 4
* JavaScript
* HTML5
* CSS
* Hangfire
* SendGrid
* Stripe
* Videosdk.live
* SignalR
* Moment.js
* FullCalendar
* TinyMCE
* HtmlSanitizer
* Azure Cognitive Services
* Azure Face Recognition API

## Database Diagram
**[URL](https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/DatabaseDiagram.png)**

<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/DatabaseDiagram.png?raw=true" alt="SH">

## Screenshots and description
* Home page for unregistered users:

<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture1.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture2.png?raw=true">

* Registration page. It consists of several filling options:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture3.png?raw=true">

* Login page:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture4.png?raw=true">

* Home page for registered users:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture5.png?raw=true">

* Stripe page for teachers. It is displayed immediately after registration, and until completed, teachers cannot go to other pages. Its purpose is to obtain data about the bank account on which the teacher will receive his monthly income:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture6.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture7.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture8.png?raw=true">

* Student balance page - from, where they can deposit to their account in the application:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture9.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture10.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture11.png?raw=true">

* Stripe page for students - they need to enter payment details to make the payment and deposit money in their account:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture12.png?raw=true">

* Teachers' balance page - in which they can change their tariff, according to which they charge for a consultation per hour (Registered consultation would get charged at the then tariff):
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture13.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture14.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture15.png?raw=true">

* Page with all teachers in a particular subject:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture16.png?raw=true">

* Teacher details page:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture17.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture18.png?raw=true">

* Page to add a review for a specific teacher by student:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture19.png?raw=true">

* Student consultation booking page:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture20.png?raw=true">

* Page with all student consultations:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture21.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture22.png?raw=true">

* Video consultation page:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture23.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture24.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture25.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture26.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture27.png?raw=true">

* Admin pages:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture28.png?raw=true">

* Page with all teachers for approval:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture29.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture30.png?raw=true">

* Details page for still unapproved teacher:
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture31.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture32.png?raw=true">

* Admin Hanfire Dashboard
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture33.png?raw=true">
<img src="https://github.com/DDeveloperBG/StudentsHelperRepoGallery/blob/master/Picture34.png?raw=true">

## Credits
  
 Using ASP.NET-MVC-Template originally developed by:
- [Nikolay Kostov](https://github.com/NikolayIT)
- [Vladislav Karamfilov](https://github.com/vladislav-karamfilov)

## :v: Show your opinion

Give a :star: if you like this project!

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Author

- [Daniel Yordanov](https://github.com/DDeveloperBG)
