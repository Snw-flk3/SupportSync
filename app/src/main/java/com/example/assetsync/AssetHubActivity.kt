package com.example.assetsync

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.widget.Button
import android.widget.LinearLayout
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.google.firebase.firestore.FirebaseFirestore

class AssetHubActivity : AppCompatActivity() {

    private lateinit var firestore: FirebaseFirestore
    private lateinit var assetsContainer: LinearLayout
    private lateinit var welcomeMessage: TextView
    private var employeeId: String = "EmployeeId"

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_assethub)

        firestore = FirebaseFirestore.getInstance()
        assetsContainer = findViewById(R.id.assetsContainer)
        welcomeMessage = findViewById(R.id.welcome_message)

        employeeId = intent.getStringExtra("EmployeeId") ?: "users"
        employeeId = intent.getStringExtra("EmployeeId") ?: "assets"

        if (employeeId.isNotEmpty()) {
            fetchUserDetails()
            fetchAssets()
        } else {
            Toast.makeText(this, "Employee ID not provided", Toast.LENGTH_SHORT).show()
        }
    }

    private fun fetchUserDetails() {
        firestore.collection("users")
            .whereEqualTo("EmployeeId", employeeId)
            .get()
            .addOnSuccessListener { documents ->
                if (!documents.isEmpty) {
                    val document = documents.first()
                    val firstName = document.getString("FirstName") ?: "users"
                    val lastName = document.getString("LastName") ?: "users"
                    welcomeMessage.text = "Welcome, $firstName $lastName"
                    Log.d("AssetHubActivity", "User found: $firstName $lastName")
                } else {
                    Toast.makeText(this, "User details not found", Toast.LENGTH_SHORT).show()
                }
            }
            .addOnFailureListener { exception ->
                Log.e("AssetHubActivity", "Error fetching user details: ${exception.message}")
            }
    }

    private fun fetchAssets() {
        firestore.collection("assets")
            .whereEqualTo("EmployeeId", employeeId)
            .get()
            .addOnSuccessListener { documents ->
                if (documents.isEmpty) {
                    Toast.makeText(this, "No assets found for this user.", Toast.LENGTH_SHORT).show()
                } else {
                    for (document in documents) {
                        val assetCategory = document.getString("assetCategory") ?: "N/A"
                        val assetMake = document.getString("assetMake") ?: "N/A"
                        val assetModel = document.getString("assetModel") ?: "N/A"
                        val assetCode = document.getString("assetCode") ?: "N/A"

                        Log.d("AssetHubActivity", "Asset found: Category=$assetCategory, Make=$assetMake, Model=$assetModel, Code=$assetCode")

                        addAssetView(assetCategory, assetMake, assetModel, assetCode)
                    }
                }
            }
            .addOnFailureListener { exception ->
                Log.e("AssetHubActivity", "Error fetching assets: ${exception.message}")
            }
    }

    private fun addAssetView(assetCategory: String, assetMake: String, assetModel: String, assetCode: String) {
        val assetLayout = LayoutInflater.from(this).inflate(R.layout.asset_item, assetsContainer, false)

        val txtAssetCategory = assetLayout.findViewById<TextView>(R.id.txtAssetCategory)
        val txtAssetMake = assetLayout.findViewById<TextView>(R.id.txtAssetMake)
        val txtAssetModel = assetLayout.findViewById<TextView>(R.id.txtAssetModel)
        val btnReportIssue = assetLayout.findViewById<Button>(R.id.btnReportIssue)

        txtAssetCategory.text = assetCategory
        txtAssetMake.text = assetMake
        txtAssetModel.text = assetModel

        btnReportIssue.setOnClickListener {
            val intent = Intent(this,ReportIssueActivity::class.java)
            intent.putExtra("assetCode", assetCode)
            startActivity(intent)
        }

        assetsContainer.addView(assetLayout)
    }
}
