README for the MySQL DB files

General process for managing these files:
    1. Update DB using the InsigniaDB_Dev schema
    2. Synchronise changes back to the model file
    3. Reverse engineer the SQL script for Dev and Prod (make sure the correct schema name is used in script)