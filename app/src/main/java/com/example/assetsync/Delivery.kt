package com.example.assetsync

// Data class to represent a delivery
data class Delivery(
    val deliveryId: String,
    val driverId: String,
    val location: String,
    val quantity: String,
    val image: String,
    val status: String = "confirmed",
    val timestamp: Long = System.currentTimeMillis() // Example additional field
)

