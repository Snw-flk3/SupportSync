package com.example.assetsync

import android.app.Activity
import android.content.Intent
import android.graphics.Bitmap
import android.net.Uri
import android.os.Bundle
import android.provider.MediaStore
import android.util.Log
import android.widget.Button
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.google.firebase.firestore.FirebaseFirestore
import com.google.firebase.storage.FirebaseStorage
import java.util.*

class StockHubDeliveryCompletionActivity : AppCompatActivity() {

    private lateinit var db: FirebaseFirestore
    private lateinit var storage: FirebaseStorage
    private lateinit var imageView: ImageView
    private var capturedImageUri: Uri? = null
    private var deliveryId: String = ""

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_stockhub_delivery_completed)

        db = FirebaseFirestore.getInstance()
        storage = FirebaseStorage.getInstance()

        deliveryId = intent.getStringExtra("deliveryId") ?: ""

        val textViewLocation: TextView = findViewById(R.id.textViewLocation)
        val textViewDriverId: TextView = findViewById(R.id.textViewDriverId)
        val textViewQuantity: TextView = findViewById(R.id.textViewQuantity)
        imageView = findViewById(R.id.imageViewDelivery)

        retrieveDriverDetails(deliveryId) { driverId, quantity, location ->
            textViewDriverId.text = "Driver ID: ${driverId ?: "N/A"}"
            textViewQuantity.text = "Quantity of Boxes: ${quantity ?: 0}"
            textViewLocation.text = "Location: $location"
        }

        val buttonCaptureImage: Button = findViewById(R.id.buttonCaptureImage)
        val buttonConfirmDelivery: Button = findViewById(R.id.buttonConfirmDelivery)
        val buttonLogout: Button = findViewById(R.id.buttonLogout)

        buttonCaptureImage.setOnClickListener {
            captureImage()
        }

        buttonConfirmDelivery.setOnClickListener {
            if (capturedImageUri != null) {
                uploadImageToFirebase()
            } else {
                showToast("Please capture an image before confirming delivery.")
            }
        }

        buttonLogout.setOnClickListener {
            val intent = Intent(this, LogInActivity::class.java)
            intent.flags = Intent.FLAG_ACTIVITY_CLEAR_TOP or Intent.FLAG_ACTIVITY_NEW_TASK
            startActivity(intent)
            finish()
        }
    }

    private fun captureImage() {
        val takePictureIntent = Intent(MediaStore.ACTION_IMAGE_CAPTURE)
        if (takePictureIntent.resolveActivity(packageManager) != null) {
            startActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE)
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == REQUEST_IMAGE_CAPTURE && resultCode == Activity.RESULT_OK) {
            val imageBitmap = data?.extras?.get("data") as Bitmap
            capturedImageUri = getImageUriFromBitmap(imageBitmap)
            imageView.setImageBitmap(imageBitmap)
        }
    }

    private fun getImageUriFromBitmap(bitmap: Bitmap): Uri {
        val path = MediaStore.Images.Media.insertImage(contentResolver, bitmap, "CapturedImage", null)
        return Uri.parse(path)
    }

    private fun uploadImageToFirebase() {
        capturedImageUri?.let { uri ->
            val storageRef = storage.reference.child("images/${UUID.randomUUID()}.jpg")
            storageRef.putFile(uri)
                .addOnSuccessListener {
                    storageRef.downloadUrl.addOnSuccessListener { downloadUrl ->
                        updateDeliveryImageUrl(downloadUrl.toString())
                    }
                }
                .addOnFailureListener {
                    showToast("Image upload failed.")
                    Log.e("StockHubDelivery", "Error uploading image: ${it.message}")
                }
        }
    }

    private fun updateDeliveryImageUrl(url: String) {
        if (deliveryId.isNotEmpty()) {
            db.collection("stock").document(deliveryId)
                .update("imageUrl", url)
                .addOnSuccessListener {
                    showToast("Delivery confirmed with image!")
                }
                .addOnFailureListener {
                    showToast("Failed to update delivery with image URL.")
                    Log.e("StockHubDelivery", "Error updating Firestore: ${it.message}")
                }
        }
    }

    private fun retrieveDriverDetails(deliveryId: String, callback: (String?, Int?, String?) -> Unit) {
        if (deliveryId.isNotEmpty()) {
            db.collection("stock").document(deliveryId).get()
                .addOnSuccessListener { snapshot ->
                    if (snapshot.exists()) {
                        val driverId = snapshot.getString("driverId")
                        val quantity = snapshot.getLong("quantity")?.toInt()
                        val location = snapshot.getString("location")
                        callback(driverId, quantity, location)
                    } else {
                        showToast("Delivery record not found.")
                        callback(null, null, null)
                    }
                }
                .addOnFailureListener {
                    showToast("Error retrieving delivery details.")
                    Log.e("StockHubDelivery", "Error: ${it.message}")
                    callback(null, null, null)
                }
        } else {
            showToast("Invalid delivery ID.")
            callback(null, null, null)
        }
    }

    private fun showToast(message: String) {
        Toast.makeText(this, message, Toast.LENGTH_SHORT).show()
    }

    companion object {
        const val REQUEST_IMAGE_CAPTURE = 1
    }
}
