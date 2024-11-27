
README
---

# SupportSync Project  
**Empowering NGOs with Efficient Management Solutions**  

## Overview  
SupportSync is an in-house management platform developed to streamline inventory and asset management for Hope Worldwide South Africa. It consists of two parts:  
- A **web-based platform** for managing inventory and assets.  
- An **Android application** for delivery management and on-the-go asset tracking.

This platform was created to help Hope Worldwide focus more on community impact by simplifying operational tasks.

---

## Team Members  

- **Wavhudi Tshibubudze**  
  Role: Administrative Lead / Android Backend Lead  
  - Ensured secure documentation, client communication, and app backend functionality.  

- **Luyanda Nkosi**  
  Role: UI/UX Designer / Web Developer  
  - Designed user-friendly interfaces and developed a responsive and visually appealing website.  

- **Morena Nkosi**  
  Role: Frontend Lead / App Backend Support  
  - Implemented engaging UI and supported backend processes for the Android application.  

- **Sethabile Dlamini**  
  Role: Unit Tester / Bug Fixer  
  - Maintained app reliability by testing, debugging, and streamlining development workflows.  

- **Precious Mpayela**  
  Role: Unit Tester / DevSecOps Assistant  
  - Ensured code quality and security through automation and secure coding practices.

---

## Web Application  

### Features  
1. **Asset Library**:  
   Manage and track organizational assets easily.  

2. **Inventory Dashboard**:  
   Get an overview of stock levels and movement.  

3. **Delivery Confirmation**:  
   Securely confirm deliveries with passcodes, similar to Uber's method.  

4. **Reports and Requests Page**:  
   View and manage asset-related reports and employee requests in one place.  

5. **Responsive Design**:  
   Works well on desktops, tablets, and mobile devices.

### Technologies Used  
- Development Environment: **Visual Studio**  
- Backend: **ASP.NET Core**  
- Frontend: **HTML, CSS, Bootstrap**  
- Database: **Firebase Firestore**  

---

## Android Application  

### Features  
1. **Delivery Management**:  
   Drivers confirm deliveries using passcodes, reducing disputes.  

2. **Inventory Management**:  
   View and update inventory data while on the go.  

3. **Asset Tracking**:  
   Provides detailed information about organizational assets.  

4. **User-Friendly Interface**:  
   Easy to use, even for those with minimal technical skills.  

### Technologies Used  
- Development Environment: **Android Studio**  
- Programming Language: **Kotlin**  
- Database: **Firebase Firestore**  

---

## How to Run the Web Application  

### Step 1: Set Up the Development Environment  
1. **Install Visual Studio**:  
   - Download and install [Visual Studio](https://visualstudio.microsoft.com/).  
   - During installation, select the **ASP.NET and web development** workload.  

2. **Install .NET SDK**:  
   - Download and install the [.NET SDK](https://dotnet.microsoft.com/download).  

3. **Clone the Project**:  
   - Obtain the project files from the repository or zip file.  
   - Extract the files to a folder on your computer.  

---

### Step 2: Open the Project in Visual Studio  
1. Open Visual Studio.  
2. Click **File > Open > Project/Solution**.  
3. Navigate to the project folder and select the `.sln` file (e.g., `HopeWorldWide.sln`).  

---

### Step 3: Configure Firebase  
1. Open the `appsettings.json` file in the project.  
2. Add your Firebase Firestore credentials. If you don’t have this file, contact the project administrator.  

---

### Step 4: Run the Application  
1. Click the green **Run** button or press `F5`.  
2. Visual Studio will open the web application in your default browser.  
3. Explore features like the Asset Library, Delivery Confirmation, and Reports pages.  

---

## How to Run the Android Application  

### Step 1: Set Up the Development Environment  
1. **Install Android Studio**:  
   - Download and install [Android Studio](https://developer.android.com/studio).  

2. **Clone the Project**:  
   - Obtain the project files from the repository or zip file.  
   - Extract the files to a folder on your computer.  

---

### Step 2: Open the Project in Android Studio  
1. Open Android Studio.  
2. Click **File > Open**.  
3. Navigate to the folder containing the project and select it.  

---

### Step 3: Configure Firebase  
1. Open the `google-services.json` file in the project.  
2. Replace it with the Firebase configuration file for your project.  
   - You can obtain this from the Firebase Console for your app.  

---

### Step 4: Run the Application  
1. Connect your Android phone or set up an emulator in Android Studio:  
   - Go to **Device Manager** and create a virtual device (if you don’t have a physical device).  
2. Click the green **Run** button in Android Studio.  
3. The app will install and launch on the connected device or emulator.  

---

## Frequently Asked Questions  

### Q: I see an error when running the web application.  
- Ensure you’ve added the correct Firebase credentials in `appsettings.json`.  
- Check if you’ve installed the required dependencies via Visual Studio’s **NuGet Package Manager**.

### Q: The Android app doesn’t load data.  
- Verify the Firebase configuration in `google-services.json`.  
- Ensure your Firebase database rules allow read/write access (for testing purposes only).  

### Q: How do I update the project?  
- Web App: Pull the latest changes using Git and update dependencies in Visual Studio.  
- Android App: Sync the project with Gradle in Android Studio.

---

## Contribution  

Contributions are welcome!  

1. Fork the repository.  
2. Create a branch (`git checkout -b feature-name`).  
3. Commit changes (`git commit -m "Add feature"`).  
4. Push to the branch (`git push origin feature-name`).  
5. Open a pull request for review.  

For issues or suggestions, email us at **supportsync@hopeworldwide.org**.

---

## Acknowledgments  

Special thanks to **Hope Worldwide South Africa** for their collaboration and support.  

---

By **SupportSync** - Empowering NGOs for a Brighter Future.

GitHub Repositories
*Website =https://github.com/VCSTDN2024/xbcad7319-poe-laundrycoders.git 
*Application = https://github.com/Snw-flk3/SupportSync.git
 