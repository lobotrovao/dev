Wex.PurchaseTransaction API

This document describes the HTTP API endpoints exposed by the Wex.PurchaseTransaction sample solution.

Base path
- All endpoints are under: /api/purchases

Endpoints

1) GET /api/purchases/
   - Description: Returns a paginated list of purchases.
   - Query parameters:
     - paginationModel: JSON string with { "page": number, "pageSize": number }
     - sortModel: string (optional) - sorting expression (project-specific) - useful for frontend as well
	 # 📑 Ordenação de Campos (Sort Parameters)

## 🔧 Exemplo de Payload

```json
[
  {
    "field": "country",
    "sort": "asc"
  }
]
```
     - filterModel: string (optional) - filtering expression (project-specific) - useful for frontend as well
## 🔍 Exemplo de Filtros Avançados

```json
{
  "items": [
    {
      "field": "",
      "operator": "equals",
      "value": ""
    },
    {
      "field": "",
      "operator": "contains",
      "value": ""
    }
  ],
  "logicOperator": "and",
  "quickFilterValues": [""],
  "quickFilterLogicOperator": "or"
}
```
   - Response: 200 OK with PaginationResponse<PurchaseDto>
     - PurchaseDto: { Id, Description, TransactionDate, PurchaseAmount }
   - Example (curl):
     curl -G "http://localhost:5000/api/purchases/" --data-urlencode "paginationModel={\"page\":0,\"pageSize\":10}" \
       --data-urlencode "sortModel=" --data-urlencode "filterModel="

2) GET /api/purchases/{purchaseId}?currency=XXX
   - Description: Returns purchase details and converted amount for the requested currency.
   - Route parameters:
     - purchaseId: long (required)
   - Query parameters:
     - currency: target currency code (required) eg. Brazil-Real
   - Response:
     - 200 OK with PurchaseConversionDto: { Id, Description, TransactionDate, PurchaseAmount, ExchangeRate }
     - 404 NotFound if purchase does not exist
   - Example (curl):
     curl "http://localhost:5000/api/purchases/5?currency=USD"

3) POST /api/purchases/
   - Description: Creates a new purchase.
   - Headers:
     - x-requestid: GUID (required) — used for idempotency
   - Body (JSON): CreatePurchaseCommand
     - { "description": "string", "transactionDate": "2023-01-01T00:00:00Z", "purchaseAmount": 12.34 }
   - Response:
     - 200 OK with created purchase id (long)
     - 400 BadRequest if the x-requestid header is missing or invalid
     - 500 Problem if creation fails
   - Example (curl):
     curl -X POST "http://localhost:5000/api/purchases/" \
       -H "Content-Type: application/json" \
       -H "x-requestid: 00000000-0000-0000-0000-000000000001" \
       -d '{"description":"test","transactionDate":"2023-01-01T00:00:00Z","purchaseAmount":10.5}'

Data contracts (important types)
- PurchaseDto: { long Id, string Description, DateTime TransactionDate, decimal PurchaseAmount }
- PurchaseConversionDto: PurchaseDto plus ExchangeRateDto
- ExchangeRateDto: { string Currency, decimal Rate, decimal ConvertedAmount, DateOnly ExchangeRateDate }

Running locally
- Build: dotnet build
- Run API: dotnet run --project Wex.PurchaseTransaction.Api/Wex.PurchaseTransaction.Api.csproj
- Tests: dotnet test

Notes
- The API uses a mediator pattern; request handling and validation occur in the Application layer.
- The POST endpoint requires a stable x-requestid header for idempotency.
