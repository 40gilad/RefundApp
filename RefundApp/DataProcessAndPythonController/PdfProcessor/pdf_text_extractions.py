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


def extract_text_till_sentence(text, end_sentence):
    start_indx = text.find(end_sentence)
    return text[:start_indx] if start_indx != -1 else ""


def extract_text_from_sentence(text, end_sentence):
    start_indx = text.find(end_sentence)
    return text[start_indx:] if start_indx != -1 else ""

def txt_from_pdf(file_path,start_sentence):

    with pdfplumber.open(file_path):
        relevant_text = extract_text_from_pdf_from_sentence(file_path, key_sentences["wolt_to_rest_beggining"])

        if not relevant_text:
            raise ValueError("The specified text was not found in the PDF.")