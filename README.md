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
