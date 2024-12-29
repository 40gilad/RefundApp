from datetime import datetime


def print_reverse(text):
    print("\n".join(line[::-1] for line in text.splitlines()))


def listify_each_record(text):
    listed = list()
    for l in text.split("\n"):
        try:
            if l[0] == '1':
                listed.append(l)
        except IndexError:
            return listed
    return listed


def normilize_name(record, customer_index, is_hebrew):
    if is_hebrew:
        customer_name = f'{record[customer_index - 1]} {record[customer_index - 2]}'
        customer_name = customer_name[::-1]
        first_name = customer_name.split(" ")[1].replace(".", "")
        last_name = customer_name.split(" ")[0].replace(".", "")
    else:
        customer_name = f'{record[customer_index + 1]} {record[customer_index + 2]}'
        first_name = customer_name.split(" ")[0].replace(".", "")
        last_name = customer_name.split(" ")[1].replace(".", "")
    return f'{first_name} {last_name}'


def get_customer_index(record):
    is_hebrew = False
    try:
        customer_index = record.index("Customer:")
        # english
    except ValueError:
        try:
            customer_index = record.index(":Customer")
            is_hebrew = True
            # hebrew
        except ValueError:
            try:
                customer_index = record.index(":Customer:")
                # not sure
                kaki = 1
            except ValueError:
                raise ValueError(f"no customer found in record \n" + str(record))
    return {'customer_index': customer_index, 'is_hebrew': is_hebrew}


def get_customer_name(record):
    try:
        name = normilize_name(record, **get_customer_index(record))
    except ValueError:
        return ""
    return name


def dictify_relevant_data_from_record(text):
    ret_list = list()
    listed = listify_each_record(text)
    for l in listed:
        curr_line = l.split(" ")
        try:
            amount = curr_line[3]
            if amount[0] == "-":
                amount = amount[1:]
            customer_name = get_customer_name(curr_line)
            try:
                refund_date = datetime.strptime(curr_line[16], f'%m/%d/%Y').date()
            except ValueError:
                refund_date = datetime.strptime(curr_line[16], f'%d.%m.%Y').date()
            ret_list.append({"order_id": curr_line[4],
                             "customer": customer_name,
                             "amount": int(float(amount)),
                             "refund_date": refund_date})
        except IndexError:
            continue
    return ret_list
