import pandas as pd
from sklearn.tree import DecisionTreeClassifier
from sklearn.model_selection import train_test_split
from sklearn.metrics import classification_report
import joblib

# Load the updated dataset
data = pd.read_excel(r'D:\\NutriGenius\\nutrigenius\\src\\Components\\Assets\\NutriGenius.xlsx')

# Map disease levels to numeric values
data['Diabetes'] = data['Diabetes'].map({'None': 0, 'Level 1 (70-99 mg/dL)': 1, 'Level 2 (100-125 mg/dL)': 2, 'Level 3 (126 mg/dL or Higher)': 3})
data['Cholesterol'] = data['Cholesterol'].map({'None': 0, 'Level 1 (Less than 200 mg/dL)': 1, 'Level 2 (200-239 mg/dL)': 2, 'Level 3 (240mg/dL and Above)': 3})
data['Thyroid Diseases'] = data['Thyroid Diseases'].map({'None': 0, 'Level 1 (Thyroid-Stimulating Hormone (TSH))': 1, 'Level 2 (Free Thyroxine (Free T4))': 2, 'Level 3 (Free Triiodothyronine (Free T3))': 3})
data['Heart Diseases'] = data['Heart Diseases'].map({'None': 0, 'Level 1 (Heart Rate 60bpm-100bpm)': 1, 'Level 2 (Blood Pressure 120/80 mmHg)': 2, 'Level 3 (Ejection Fraction 55%-70%)': 3})
data['Depression'] = data['Depression'].map({'None': 0, 'Level 1 (Mild Depression)': 1, 'Level 2 (Moderate Depression)': 2, 'Level 3 (Severe Depression)': 3})

# Define features (X) and target (y)
X = data[['Age', 'BMI', 'Diabetes', 'Cholesterol', 'Thyroid Diseases', 'Heart Diseases', 'Depression']]
y = data['Diet Plan']

# Split the data: 80% training, 20% testing
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Train the model with hyperparameters tuned to improve performance
model = DecisionTreeClassifier(
    max_depth=10,             # Limits depth to control complexity
    min_samples_split=5,      # Controls minimum samples needed to split a node
    min_samples_leaf=2,       # Controls minimum samples needed at leaf node
    class_weight='balanced',  # Gives weight to underrepresented classes if needed
    random_state=42
)

# Fit the model with the updated dataset
model.fit(X_train, y_train)

# Evaluate the model on the test set to check if it handles disease levels correctly
y_pred = model.predict(X_test)
print(classification_report(y_test, y_pred))

# Save the retrained model to a file
joblib.dump(model, r'D:\\NutriGenius\\Model\\diet_plan_model.pkl')
