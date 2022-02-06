# StudentsHelper
Web Application to help students with their homework, lessons, etc. With immediate consultations about the topic made with a professional teacher on the subject.

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

## Used Frameworks

* ASP.Net 6
* Entity Framework 6
* Hangfire
* SendGrid
* Stripe
* Videosdk.live
* Moment.js

## Pictures


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