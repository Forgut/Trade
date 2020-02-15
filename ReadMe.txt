0. Trade - za�o�enia projektowe. 

Aplikacja wzrouje si� na systemie handlu obecnym w grze Europa Universalis IV. Za�o�eniem tej aplikacji nie jest dok�adne
kopiowanie zasad panuj�cych w systmie gry, a jedynie pr�ba jak najlepszego odtworzenia tego systemu. W trakcie tworzenia 
przyj�te zosta�o wiele uproszcze�, kt�re normalnie wynikaj� ze skomplikowanych mechanik gry, kt�rych nie planowa�em tutaj 
implementowa�. Aplikacja rzyjmuje form� konsolow�.

1. Funkcje aplikacji

Aplikacja odwzorowuje nastepuj�cy mechanizm:
Istnieje sie� w�z��w handlowych (TradeNode). 

	TradeNode:
		+ Incoming - lista w�z��w, kt�re przekierowuj� handel do tego w�z�a
		+ Outgoing - lista w�z��w, do kt�rych przekierowuje handel ten w�ze�
		+ Value - warto�� w�z�a
		+ TradePower - si�a handolwa w�z�a (potrzebna przy obliczaniu podzia�u mi�dzy aktywne w w�le pa�stwa)
		+ Countries - lista kraj�w, kt�re maj� udzia� w TradePower w�z�a
		+ Provinces - lista prowincji, kt�re wchodz� w sk�ad w�z�a, buduj�c tym samym jego Value i TradePower
		+ Merchants - lista kupc�w, kt�rzy znajduj� si� w w�le

W�z�y po��czone s� ze sob� wzajemnie, tworz�c acykliczny graf skierowany.
Ka�dy w�ze� posiada wi�c po��czenie z conajmniej jednym innym w�z�em.

	TradeNode A -->-->--> TradeNode B
	
	Ozancza to, �e A dla B jest w�z�em w puli Incoming, a B dla A Outgoing.

Kierunek po��czenia w�z��w ma znaczenie poniewa� okre�la on, kto do kogo przesy�a pieni�dze.

W�z�y zawieraj� w sobie prowincje. 
	
	Province:
		+ TradePower - warto�� handlowa prowincji w w�le
		+ TradeValue - warto�� pieni�na prowincji w w�le
		+ Owner - kraj, kt�ry jest w�a�cicielem prowincji

	Przyk�adowo:
		W sk�ad w�z�a W wchodz� 4 prowincje:
			- A : TradePower = 4, TradeValue = 7, Owner = X
			- B : TradePower = 3, TradeValue = 4, Owner = X
			- C : TradePower = 1, TradeValue = 4, Owner = Y
			- D : TradePower = 2, TradeValue = 5, Owner = Z
		Oznacza to wi�c, �e w�ze� A posiada:
			TradePower = 10 (suma wszystkich sk�adowych prowincji)
			TradeValue = 20 (suma wszystkich sk�adowych prowincji)
		Podzia� si�y w w�le (bez zewn�trznych modyfikator�w) b�dzie wygl�da� nastepuj�co:
			X - 7/10 = 70% trade power w w�le
			Y - 1/10 = 10% trade power w w�le
			Z - 2/10 = 20% trade power w w�le
		Oznacza to, �e X decyduje o tym co si� dzieje z 70% Value w�z�a, Y decyduje o 10% a Z o 20%.
		Decyzja oznacza jedno z dw�ch:
			- Zbiera swoj� cz�� pieni�dze i zarabia na tym na miejscu
			- Steruje swoj� warto�� do wybranego w�z�a z puli Outgoing, by doda� t� warto�� do tamtego w�z�a. 

W w�z�ach, kraje mog� umie�ci� swoich kupc�w. Kupiec dodaje troch� TradePower do w�z�a, na korzy�� kraju, kt�ry go tam umie�ci�,
oraz pozwala na wyb�r - czy w w�le zbiera pieni�dze czy przekieruje je dalej w d� strumienia.

	Merchant
		+ Country
		+ Action - zbiera (Collect) albo transferuje (Transfer)

---Obliczanie warto�ci w w�le---
Proces ten dzielony jest na 2 fazy.
	Faza 1: Podzia�
		Ze stosunku TradePower wszystkich graczy Collectuj�cych w TradeNode do ca�ego TradePower w�z�a, obliczana jest warto��,
		kt�ra zostaje w w�le. Pozosta�a warto��, to warto�� b�d�ca przesy�ana dalej do kolejnych w�z��w.

		TradeNodeValue = 100
		TradePower = 100

		Przyk�adowo:
		U - (20) 20% trade power w w�le
		W - (30) 30% trade power w w�le
		X -	(10) 10% trade power w w�le
		Y -	(15) 15% trade power w w�le
		Z - (25) 25% trade power w w�le

		U,W,X collect
		Y,Z - transfer

		Po fazie pierwszej w w�le zostaje 60 Value, a 40 b�dzie transferowane dalej.

	Faza 2: Zbieranie
		Dzielimy graczy na dwie grupy: Collect i Trasnfering. Teraz podzia� TradePower odbywa si� mi�dzy nimi.
		Ka�dy gracz z grupy collect oblicza sw�j udzia� na podstawie tego jaki ma tradePower w stosunku do innych graczy
		w grupie collect. Czyli:
		Suma TradePower wszystkich collectuj�cych w w�le to 60.
		U posiada wi�c 20/60 czyli 33% TradePower wi�c zbierze on 33% z 60 czyli 20.
		W posiada wi�c 30/60 czyli 50% TradePower wi�c zbierze on 50% z 60 czyli 30.
		X posiada wi�c 10/60 czyli 16% TradePower wi�c zbierze on 16% z 60 czyli 10.
		W grupie transfering z kolei:
		Suma TradePower wszywstkich tranferuj�cych to 40.
		Y posiada 15/40 czyli 37% TradePower wi�c przetransferuje on 37% z 40 czyli 15.
		Z posiada 25/40 czyli 63% TradePower wi�c przetransferuje on 63% z 40 czyli 25.

Przyk�adowa sytuacja:
	W�ze� A:
		TradePower - 10 (X posiada 40%, Y posiada 60%) 
		TradeValue - 20
	W�ze� B:
		TradePower - 10 (X posiada 70%, Y posiada 30%)
		TradeValue - 20
	A -->-->--> B 
	Gracz X decyduje, �e w w�le A steruje handel do w�z�a B i tam zbiera warto��.
	Gracz Y decyduje, �e w obydwu w�z�ach zbiera on pieni�dze.

	Sytuacja wi�c wygl�da� b�dzie nast�puj�co:
	W�ze� A:
		TradePower - 10 (X posiada 40%, Y posiada 60%) 
		TradeValue - 12 (8 jest transferowane do B)
	W�ze� B:
		TradePower - 10 (X posiada 70%, Y posiada 30%)
		TradeValue - 28 (8 przysz�o z A)
	X zbiera w B 70% z 28 czyli 19.6.
	Y zbiera w A 12 (bo tylko on tam zbiera) i w B 8.4 (bo tutaj dzieli si� pul� z X) czyli ��cznie 20.4.
	TradePower s�u�y do podzia�u puli mi�dzy graczy a nast�pnie ta pula przek�ada si� na warto�ci pieni�ne node'a.

Przyk�adowa sytuacja 2:
	W�ze� A:
		TradePower - 10 (U posiada 20%, W posiada 20%, X posiada 20%, Y posiada 20%, Z posiada 20%)
		TradeValue - 20
	Wszyscy poza U decyduj� si� kierowa� handel w d� strumienia.
	Oznacza to, �e w w�le A zostanie 4, z kt�rego 100% zbierze U.
	Z w�z�a A przekierowane zostanie 16.