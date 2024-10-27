from flask import Flask, request, jsonify
import json

app = Flask(__name__)


def compare_refunds(wolt_data, restaurant_data):
    # Create dictionaries for easy lookup by order ID
    wolt_refunds = {order['order_id']: order['refund_amount'] for order in wolt_data}
    restaurant_refunds = {order['order_id']: order['refund_amount'] for order in restaurant_data}

    # Compare refunds between WOLT and restaurant
    discrepancies = []

    # Check for mismatched refund amounts and missing orders
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
    # Get the JSON data from the POST request
    wolt_data = request.json.get('wolt_data')
    restaurant_data = request.json.get('restaurant_data')

    # Perform comparison
    discrepancies = compare_refunds(wolt_data, restaurant_data)

    # Return the result as JSON
    return jsonify(discrepancies)


if __name__ == '__main__':
    app.run(debug=True,host='localhost')
