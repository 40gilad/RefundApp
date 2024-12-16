import pdfplumber


def extract_text_from_pdf_from_sentence(pdf_file, start_sentence, to_end_of_file=True, search_reverse=True):
    """
    Extract text from a PDF starting from the specified word.

    :param pdf_file: Path to the PDF file to search.
    :param start_word: Starting word for extraction.
    :param to_end_of_file: If True (default), extracts text from the starting word to the end of the file.
                           If False, extracts text to the end of the current page.
    :param search_reverse: If True (default), searches pages from the last to the first.
    :return: Extracted text from the desired point onwards.
    """
    extracted_text = ""

    with pdfplumber.open(pdf_file) as pdf:
        # Determine the page range based on the search direction
        page_range = range(len(pdf.pages) - 1, -1, -1) if search_reverse else range(len(pdf.pages))

        for page_num in page_range:
            page = pdf.pages[page_num]
            text = page.extract_text()

            if text and start_sentence in text:
                # Find the starting position of the word in the text
                start_index = text.index(start_sentence)

                if to_end_of_file:
                    # Extract text from the current position to the end of the file
                    extracted_text = text[start_index:] + "\n".join(
                        pdf.pages[i].extract_text() for i in range(page_num + 1, len(pdf.pages)))
                else:
                    # Extract text from the current position to the end of the page
                    extracted_text = text[start_index:]

                return extracted_text

    return ""  # Return empty string if the word is not found


def extract_text_till_sentence(text, end_sentence='מ %"עמ מ"עמ מ״עמ אללמ״עמ ללוכהנמזה רפסמ םייוכינ'):
    start_indx = text.find(end_sentence)
    return text[:start_indx] if start_indx != -1 else ""


def extract_text_from_sentence(text, end_sentence='מ %"עמ מ"עמ מ״עמ אללמ״עמ ללוכהנמזה רפסמ םייוכינ'):
    start_indx = text.find(end_sentence)
    return text[start_indx:] if start_indx != -1 else ""


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


def dictify_relevant_data_from_record(text):
    ret_list = list()
    listed = listify_each_record(text)
    for l in listed:
        curr_line = l.split(" ")
        amount = curr_line[1]
        if amount[0] == "-":
            amount = amount[1:]
        ret_list.append({"order_id": curr_line[0], "amount": amount})
    return ret_list


def extract_json_from_last_page(file_path, sentence='מ"עמ ילב מ״עמ ללוכ הנמזה רפסמ תופסות'):
    """
    Extracts tables from the last page of the PDF, starting from a specific sentence.

    :param file_path: Path to the PDF file.
    """

    with pdfplumber.open(file_path):
        text = extract_text_from_pdf_from_sentence(file_path, sentence)

        if not text:
            raise ValueError("The specified text was not found in the PDF.")

        gifts = extract_text_till_sentence(text)
        dictify_relevant_data_from_record(gifts)
        print("#############################")
        print_reverse(extract_text_from_sentence(text))
        kaki = 1


# Example usage
res = extract_json_from_last_page("example.pdf")
