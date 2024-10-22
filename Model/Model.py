# # import pandas as pd
# from sklearn.tree import DecisionTreeClassifier
# from sklearn.model_selection import train_test_split
# from sklearn.metrics import accuracy_score
# import joblib
# import os

# # Load the dataset
# data = pd.read_excel(r'D:\NutriGenius\nutrigenius\src\Components\Assets\NutriGenius.xlsx')

# # Prepare feature matrix X and target variable y
# X = data[['Age', 'BMI', 'Diabetes', 'Cholesterol', 'Thyroid Diseases', 'Heart Diseases', 'Depression']]
# y = data['Diet Plan']

# # Split the data into training and testing sets
# X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# # Initialize the Decision Tree model
# model = DecisionTreeClassifier()

# # Train the model
# model.fit(X_train, y_train)

# # Predict on the test data
# y_pred = model.predict(X_test)

# # Evaluate the model's performance
# accuracy = accuracy_score(y_test, y_pred)
# print(f"Model Accuracy: {accuracy * 100:.2f}%")

# # Ensure the model directory exists
# model_dir = r'D:\NutriGenius\Model'
# if not os.path.exists(model_dir):
#     os.makedirs(model_dir)

# # Save the model to a file
# model_path = os.path.join(model_dir, 'diet_plan_model.pkl')
# try:
#     joblib.dump(model, model_path)
#     print(f"Model saved successfully at {model_path}")
# except Exception as e:
#     # print(f"Error saving the model: {e}")


import pandas as pd
from sklearn.tree import DecisionTreeClassifier
from sklearn.model_selection import train_test_split
import joblib

# Load the dataset
data = pd.read_excel(r'D:\\NutriGenius\\nutrigenius\\src\\Components\\Assets\\NutriGenius.xlsx')

# Ensure the disease columns are treated as numerical values
data['Diabetes'] = data['Diabetes'].map({'No': 0, 'Yes': 1})
data['Cholesterol'] = data['Cholesterol'].map({'No': 0, 'Yes': 1})
data['Thyroid Diseases'] = data['Thyroid Diseases'].map({'No': 0, 'Yes': 1})
data['Heart Diseases'] = data['Heart Diseases'].map({'No': 0, 'Yes': 1})
data['Depression'] = data['Depression'].map({'No': 0, 'Yes': 1})

# Features (X) and target (y)
X = data[['Age', 'BMI', 'Diabetes', 'Cholesterol', 'Thyroid Diseases', 'Heart Diseases', 'Depression']]
y = data['Diet Plan']

# Split the data: 80% training, 20% testing
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Train the model
model = DecisionTreeClassifier()
model.fit(X_train, y_train)

# Save the model to a file
joblib.dump(model, r'D:\\NutriGenius\\Model\\diet_plan_model.pkl')
