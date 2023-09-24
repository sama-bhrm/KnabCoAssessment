# KnabCoAssessment

### Knab.CrtyptoCurrencyExchange###

All APIs are secured with a _**static Api-Key**_, if in Appsetting.json file, the value of key _**NeedAuthentication**_ is True. (by default, the value is set to false, so there is no authentication for APIs)
If you want to use secure APIs, please do these steps:
1.	Appsetting.json file, set NeedAuthentication in section Authentication with True
2.	Set ApiKey to **434929fff6e940888120284b7118f6db** in Authorize section of Swagger or add header **"X-Api-Key"** with **434929fff6e940888120284b7118f6db**
