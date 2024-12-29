from flask import Flask, request, jsonify
import os
import sys
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

import DataProcessors.PdfProcessor.ProcessPdf as ProcessPdf
import DataProcessors.JsonProcessor.ResturantJsonProcessor as rjp
import json

app = Flask(__name__)

def preprocess_data(resturant_raw_json,file):
    restaurant_data = rjp.preprocess(json.loads(resturant_raw_json))
    wolt_data = ProcessPdf.extract_json_pdf(file)
    return restaurant_data,wolt_data

def compare_refunds(wolt_data, restaurant_data):
    # Create dictionaries for easy lookup by order ID
    print(f"WOLT:\n{wolt_data}\n----------------------------------\nRESTURANT:\n{restaurant_data}")
    wolt_refunds = {order['order_id']: order['refund_amount'] for order in wolt_data}
    restaurant_refunds = {order['order_id']: order['refund_amount'] for order in restaurant_data}
    discrepancies = []

    for order_id, wolt_refund in wolt_refunds.items():
        restaurant_refund = restaurant_refunds.get(order_id)
        if restaurant_refund is None:
            discrepancies.append({
                'order_id': order_id,
                'wolt_refund': wolt_refund,
                'status': 'Missing in Restaurant'
            })
        elif wolt_refund != restaurant_refund:
            discrepancies.append({
                'order_id': order_id,
                'wolt_refund': wolt_refund,
                'restaurant_refund': restaurant_refund,
                'status': 'Refund Mismatch'
            })

    return discrepancies

@app.route('/compare', methods=['POST'])
def compare():
    wolt_data = None
    restaurant_data = None
    try:
        file = request.files['file']
        resturant_raw_json = request.form.get('restaurant_data')

        if not file:
            return jsonify({"error": "File is required"}), 400

        if not resturant_raw_json:
            return jsonify({"error": "Restaurant data is required"}), 400

        restaurant_data , wolt_data = preprocess_data(resturant_raw_json,file)

    except Exception as e:
        # Return a generic error response for unexpected errors
        return jsonify({"error": f"An unexpected error occurred: {str(e)}"}), 500

    discrepancies = compare_refunds(wolt_data, restaurant_data)
    return jsonify(discrepancies)

if __name__ == '__main__':
    app.run(ssl_context=('C:/Users/40gil/Desktop/not_work/my_scipts/RefundApp/pems/cert.pem',
                         'C:/Users/40gil/Desktop/not_work/my_scipts/RefundApp/pems/key.pem'))


