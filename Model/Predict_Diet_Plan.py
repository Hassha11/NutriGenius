import sys
import joblib

# Load the trained model
try:
    model = joblib.load(r'D:\NutriGenius\Model\diet_plan_model.pkl')
except FileNotFoundError:
    print("Error: The model file D:\\NutriGenius\\Model\\diet_plan_model.pkl does not exist.")
    sys.exit()

# Parse command line arguments (user details passed from backend)
age = float(sys.argv[1])
bmi = float(sys.argv[2])
diabetes = int(sys.argv[3])
cholesterol = int(sys.argv[4])
thyroid = int(sys.argv[5])
heart_disease = int(sys.argv[6])
depression = int(sys.argv[7])

# Prepare the input data for prediction
input_data = [[age, bmi, diabetes, cholesterol, thyroid, heart_disease, depression]]

# Make the prediction
try:
    prediction = model.predict(input_data)
    print(prediction[0])  # Print the predicted diet plan
except Exception as e:
    print(f"Error making prediction: {e}")
    sys.exit()
