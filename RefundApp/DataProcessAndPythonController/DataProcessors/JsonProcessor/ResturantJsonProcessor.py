from datetime import datetime


def process_date(resturant_raw_json):
    for key in resturant_raw_json:
        resturant_raw_json[key]["refundDate"] = datetime.strptime(resturant_raw_json[key]["refundDate"][:10],
                                                                  '%Y-%m-%d').date()


def process_amount(resturant_raw_json):
    for key in resturant_raw_json:
        resturant_raw_json[key]["amount"] = int(float(resturant_raw_json[key]["amount"]))

def preprocess(resturant_raw_json):
    process_date(resturant_raw_json)
    process_amount(resturant_raw_json)
    return resturant_raw_json
