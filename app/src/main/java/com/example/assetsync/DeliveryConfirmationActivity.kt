package com.example.assetsync

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.google.firebase.firestore.FirebaseFirestore
import com.google.firebase.firestore.SetOptions

class DeliveryConfirmationActivity : AppCompatActivity() {

    private lateinit var db: FirebaseFirestore

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_delivery_confirmation)

        // Initialize Firestore
        db = FirebaseFirestore.getInstance()

        // Find views by ID
        val textViewDriverId: TextView = findViewById(R.id.textViewDriverId)
        val textViewQuantity: TextView = findViewById(R.id.textViewQuantity)
        val editTextEmail: EditText = findViewById(R.id.editTextEmail)
        val editTextPassword: EditText = findViewById(R.id.editTextPassword)
        val buttonCompleteDelivery: Button = findViewById(R.id.buttonConfirmDelivery)

        // Retrieve data from the intent
        val deliveryId = intent.getStringExtra("deliveryId") ?: ""
        val driverId = intent.getStringExtra("driverId") ?: ""
        val deliveryLocation = intent.getStringExtra("deliveryLocation") ?: ""
        val quantity = intent.getLongExtra("quantity", 0L) // Retrieve quantity as Long
        val imageUrl = intent.getStringExtra("imageUrl") ?: ""

        Log.d("DeliveryConfirmation", "Received Delivery ID: $deliveryId")
        Log.d("DeliveryConfirmation", "Received Driver ID: $driverId")
        Log.d("DeliveryConfirmation", "Received Delivery Location: $deliveryLocation")
        Log.d("DeliveryConfirmation", "Received Quantity: $quantity")
        Log.d("DeliveryConfirmation", "Received Image URL: $imageUrl")

        // Check if deliveryId is empty
        if (deliveryId.isEmpty()) {
            Toast.makeText(this, "Delivery ID is missing", Toast.LENGTH_SHORT).show()
            finish() // Close the activity
            return
        }

        // Populate the TextViews with the retrieved data
        textViewDriverId.text = driverId
        textViewQuantity.text = quantity.toString()

        // Set a click listener for the Complete Delivery button
        buttonCompleteDelivery.setOnClickListener {
            val email = editTextEmail.text.toString().trim()
            val password = editTextPassword.text.toString().trim()

            if (email.isNotEmpty() && password.isNotEmpty()) {
                updateDeliveryStatus(deliveryId, email, password)
            } else {
                Toast.makeText(this, "Please enter both email and password", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun updateDeliveryStatus(deliveryId: String, email: String, password: String) {
        // Create the delivery update data
        val deliveryUpdate = hashMapOf(
            "status" to "completed",
            "completedBy" to email,
            "completedPassword" to password // Store password temporarily, this is not a secure practice!
        )

        db.collection("stock").document(deliveryId)
            .set(deliveryUpdate, SetOptions.merge()) // Use SetOptions.merge() to update only specific fields
            .addOnSuccessListener {
                Toast.makeText(this, "Delivery marked as completed", Toast.LENGTH_SHORT).show()
                navigateToStockHubDeliveryCompletionActivity()
            }
            .addOnFailureListener { e ->
                Log.e("DeliveryConfirmation", "Error updating delivery status: ${e.message}")
                Toast.makeText(this, "Error marking delivery as completed", Toast.LENGTH_SHORT).show()
            }
    }

    private fun navigateToStockHubDeliveryCompletionActivity() {
        startActivity(Intent(this, StockHubDeliveryCompletionActivity::class.java))
        finish() // Close the current activity
    }
}
