# JSON Refund Comparison Tool

## Overview
This project provides a tool to compare refund data from delivery app and restaurant JSON files. It identifies discrepancies in refund amounts and generates a new JSON file containing the results.

## Features
- Compare two JSON files based on order IDs and refund amounts.
- Generate a new JSON file with:
  - Refund mismatches between delivery company and restaurant data.
  - Records that are present in delivery company but missing in the restaurant data.

## Getting Started

### Prerequisites
- Python 3.x
- Virtual environment (recommended)
- Flask (for the API)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/40gilad/RefundApp.git