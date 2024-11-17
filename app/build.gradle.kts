plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
    id("com.google.gms.google-services")
}

android {
    namespace = "com.example.assetsync"
    compileSdk = 34

    defaultConfig {
        applicationId = "com.example.assetsync"
        minSdk = 24
        targetSdk = 34
        versionCode = 1
        versionName = "1.0"
        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    buildTypes {
        release {
            isMinifyEnabled = true // Enable minification for release builds
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }

    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_1_8
        targetCompatibility = JavaVersion.VERSION_1_8
    }

    kotlinOptions {
        jvmTarget = "1.8"
    }
}

dependencies {
    // Microsoft SQL Server JDBC Driver
    implementation("com.microsoft.sqlserver:mssql-jdbc:9.2.1.jre11")

    // Image loading library (choose one)
    implementation("com.github.bumptech.glide:glide:4.15.0")
    annotationProcessor("com.github.bumptech.glide:compiler:4.15.0")
//Piscasso is commented out to avoid duplication with glide
   //implementation("com.squareup.picasso:picasso:2.71828") // Uncomment if using Picasso

    // AndroidX Libraries
    implementation("androidx.core:core-ktx:1.13.1")
    implementation("androidx.appcompat:appcompat:1.7.0")
    implementation("com.google.android.material:material:1.12.0")
    implementation("androidx.constraintlayout:constraintlayout:2.1.4")

    // Firebase dependencies

    implementation(platform("com.google.firebase:firebase-bom:32.1.0")) // Import the BoM to manage version
    implementation("com.google.firebase:firebase-auth") // Firebase Authentication
    implementation("com.google.firebase:firebase-database") // Firebase realtime database
    implementation("com.google.firebase:firebase-firestore") //Firebase firestore
    implementation("com.google.firebase:firebase-storage-ktx") // Firebase storage KTX extensions
//Updated firebase firestore and storage ktx versions are included in the BoM, no need to specify them separately
    //implementation ("com.google.firebase:firebase-firestore-ktx:24.4.1") // Update to the latest version
   // implementation ("com.google.firebase:firebase-storage-ktx:20.1.0") // Update to the latest version
   // implementation ("androidx.appcompat:appcompat:1.5.1") // Ensure this is included

    // Testing Libraries
    testImplementation("junit:junit:4.13.2")
    androidTestImplementation("androidx.test.ext:junit:1.2.1")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.6.1")
}
