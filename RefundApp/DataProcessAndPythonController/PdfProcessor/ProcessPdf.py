import text_manipulation
import pdf_text_extractions




def extract_json_pdf(file_path):

    key_sentences = {
        "wolt_to_rest_beggining": 'מ"עמ ילב מ״עמ ללוכ הנמזה רפסמ תופסות',
        "rest_to_wolt_beginning": 'מ %"עמ מ"עמ מ״עמ אללמ״עמ ללוכהנמזה רפסמ םייוכינ',
        "extra_commisions": '% מ"עמ מ"עמ מ״עמ אלל מ"עמ םע הנמזה רפסמ תופסונ תולמע'
    }

    relevant_text = (pdf_text_extractions.
                     extract_text_from_pdf_from_sentence(file_path, key_sentences["wolt_to_rest_beggining"]))

    ret_dict = dict().fromkeys(["wolt_to_rest", "rest_to_wolt"])

    wolt_to_rest = pdf_text_extractions.extract_text_till_sentence(relevant_text, key_sentences["rest_to_wolt_beginning"])
    rest_to_wolt = pdf_text_extractions.extract_text_till_sentence(
        pdf_text_extractions.extract_text_from_sentence(relevant_text, key_sentences["rest_to_wolt_beginning"]),
        key_sentences["extra_commisions"])

    ret_dict["wolt_to_rest"] = text_manipulation.dictify_relevant_data_from_record(wolt_to_rest)
    ret_dict["rest_to_wolt"] = text_manipulation.dictify_relevant_data_from_record(rest_to_wolt)
    return ret_dict


# Example usage
import json

print(json.dumps(extract_json_pdf("example.pdf"), indent=4))
