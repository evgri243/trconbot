This repository contains proof-of-concept bot made to show one of possible designs for QnA bot built on Microsoft platform.

At a higher level bot uses that logic. After the initial greeting it allows in the form of buttons select one of the prebuild scripted dialogs (e.g. price calculation) or respond with natural language. In case of a button click it starts the scenario, while a text answer triggers a call to LUIS (with translator, see GetLuisQueryTextAsync) in attempt to identify an intent and start a corresponding scenario. In the case of no intents found bot returns some static answer using QnA Maker. So, in general the bot became a sort of extended QnA with a static content served through QnA Maker and an active (with questions to clarify the answer) provided by LUIS buttons and dialogs.

To deploy it yourself, you must do the following:
1. Clone the repository locally through Git clone
2.Deploy resources to Azure by executing the Azure/deploy.ps1 script. If necessary/desired, you can change the name of the services in parameters.json. You can always see deployed services on [portal.azure.com](https://portal.azure.com/).
3. Go to [luis.ai](https://luis.ai), login, import the model from Luis/TrCon Luis.json and train the model. Add a key for Luis service from 2 and publish the model.
4. Go to [dev.botframework.com](https://dev.cotframework.com), login, and create a new bot, save AppId and Password.
*Attention! The password is shown only once!*
5. Open Solution TrConBot/TrConBot.sln and in TrConBot/TrConBot.Api/Web.config and replace the keys:
	* MicrosoftAppId with AppID from 4.
	* MicrosoftAppPassword with Password from 4.
	* BingTranslatorSubscriptionKey with one of the keys for the translator from 2.
	* In TrConBot/TrConBot.Api/Utility/AccessKeys.cs replace LuismodelId and LuisSubscriptionKey with keys from 3.
7. Run the Web application and test it with [emulator](https://docs.botframework.com/en-us/tools/bot-framework-emulator/) by changing the port to 3979 and specifying the keys from 4
8. Publish the bot by right-clicking on the web project, publish and selecting App Service from 2
9. Use dev.botframework.com to add channels for Skype and test again using Skype.


Чтобы развернуть его самостоятельно, необходимо выполнить следующее:
1.	Клонировать репозиторий локально через git clone
2.	Развернуть ресурсы в Azure, выполнив скрипт Azure/deploy.ps1. При необходимости/желании названия сервисов можно поменять в parameters.json. Увидеть развернутое можно на [portal.azure.com](https://portal.azure.com/).
3.	Перейти на [luis.ai](https://luis.ai), авторизироваться, импортировать модель из Luis/TrCon Luis.json и выполнить обучение. Добавить ключ для Luis из сервиса из 2 пункта и опубликовать модель.
4.	Перейти на [dev.botframework.com](https://dev.cotframework.com), авторизироваться и создать бота, сохранив AppId и Password. *Внимание, пароль показывается только один раз!*
5.	Открыть solution TrConBot/TrConBot.sln и в TrConBot/TrConBot.Api/Web.config заменить ключи:
	*	MicrosoftAppId на AppId из 4 пункта.
	*	MicrosoftAppPassword на Password из 4 пунта.
	*	BingTranslatorSubscriptionKey на один из ключей для переводчика из 2 пунта.
	*	В TrConBot/TrConBot.Api/Utility/AccessKeys.cs заменить LuisModelId и LuisSubscriptionKey на ключи из 3 пункта.
7.	Запустить web-приложение и проверить работоспособность с помощью [эмулятора](https://docs.botframework.com/en-us/tools/bot-framework-emulator/), изменив порт на 3979 и указав ключи из пункта 4
8.	Опубликовать web-приложение, кликнул правой кнопкой по проекту -> publish и выбрав AppService из пунта 2
9.	Через dev.botframework.com добавить бота в Skype и протестировать.
