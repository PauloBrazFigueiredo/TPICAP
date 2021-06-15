# TP ICAP Persons REST Service Exercise

## Solution Structure  
The solution, *TPICAP*, is structure by the following projects:  
1. TPICAP.API - REST service implementation;  
2. TPICAP.Data - SQL Server Express data access implementation;  
3. TPICAP.API.Tests - Unit tests used for the *TPICAP.API* implementation;
4. TPICAP.Data.Tests - Unit tests used for the *TPICAP.Data* implementation;
## Run In Visual Studio  
1. Run the *TPICAP.API*project;
2. In *swagger* run */api/authentication/login* of *Authentication* (setting *userName* and *password*);
3. Copy the *token* from the response bofy of the previous invokation;
4. Press *Authorize* button (top rigth);
5. Paste the copied token value to *value* texbox;
6. Press *Authorize* to have authorization to invoke *Persons* resources.