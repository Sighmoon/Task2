﻿# Task2 v0.6

## Новое:
<li> Консольное приложение CollectTenderXml. Данная консоль собирает документы с API и передает их в консольное приложение IndexTenderXml.
<li> Консольное приложение IndexTenderXml. Данная консоль принимает документы из очереди сообщений и обрабатывает их: высчитывает хеш xml-документа, добавляет в базу данных, если документ обновлен или не существует в базе.

Разделение задачи получения и обработки тендеров позволяет сократить время, затрачиваемое на обработку, так как теперь получение тендера независит от обработки предыдущего.