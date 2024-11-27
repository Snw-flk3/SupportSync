buildscript {
    repositories {
        google()
        mavenCentral()  // Keep only mavenCentral and google repositories
    }
    dependencies {
        classpath("com.android.tools.build:gradle:8.3.0")  // Gradle build tools
        classpath("com.google.gms:google-services:4.3.15")  // Google services classpath for Firebase
        classpath("org.jetbrains.kotlin:kotlin-gradle-plugin:1.9.22") // Align Kotlin version with plugins block
    }
}

plugins {
    id("com.android.application") version "8.3.0" apply false
    id("org.jetbrains.kotlin.android") version "1.9.22" apply false
}


