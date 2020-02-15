0. Trade - za³o¿enia projektowe. 

Aplikacja wzrouje siê na systemie handlu obecnym w grze Europa Universalis IV. Za³o¿eniem tej aplikacji nie jest dok³adne
kopiowanie zasad panuj¹cych w systmie gry, a jedynie próba jak najlepszego odtworzenia tego systemu. W trakcie tworzenia 
przyjête zosta³o wiele uproszczeñ, które normalnie wynikaj¹ ze skomplikowanych mechanik gry, których nie planowa³em tutaj 
implementowaæ. Aplikacja rzyjmuje formê konsolow¹.

1. Funkcje aplikacji

Aplikacja odwzorowuje nastepuj¹cy mechanizm:
Istnieje sieæ wêz³ów handlowych (TradeNode). 

	TradeNode:
		+ Incoming - lista wêz³ów, które przekierowuj¹ handel do tego wêz³a
		+ Outgoing - lista wêz³ów, do których przekierowuje handel ten wêze³
		+ Value - wartoœæ wêz³a
		+ TradePower - si³a handolwa wêz³a (potrzebna przy obliczaniu podzia³u miêdzy aktywne w wêŸle pañstwa)
		+ Countries - lista krajów, które maj¹ udzia³ w TradePower wêz³a
		+ Provinces - lista prowincji, które wchodz¹ w sk³ad wêz³a, buduj¹c tym samym jego Value i TradePower
		+ Merchants - lista kupców, którzy znajduj¹ siê w wêŸle

Wêz³y po³¹czone s¹ ze sob¹ wzajemnie, tworz¹c acykliczny graf skierowany.
Ka¿dy wêze³ posiada wiêc po³¹czenie z conajmniej jednym innym wêz³em.

	TradeNode A -->-->--> TradeNode B
	
	Ozancza to, ¿e A dla B jest wêz³em w puli Incoming, a B dla A Outgoing.

Kierunek po³¹czenia wêz³ów ma znaczenie poniewa¿ okreœla on, kto do kogo przesy³a pieni¹dze.

Wêz³y zawieraj¹ w sobie prowincje. 
	
	Province:
		+ TradePower - wartoœæ handlowa prowincji w wêŸle
		+ TradeValue - wartoœæ pieniê¿na prowincji w wêŸle
		+ Owner - kraj, który jest w³aœcicielem prowincji

	Przyk³adowo:
		W sk³ad wêz³a W wchodz¹ 4 prowincje:
			- A : TradePower = 4, TradeValue = 7, Owner = X
			- B : TradePower = 3, TradeValue = 4, Owner = X
			- C : TradePower = 1, TradeValue = 4, Owner = Y
			- D : TradePower = 2, TradeValue = 5, Owner = Z
		Oznacza to wiêc, ¿e wêze³ A posiada:
			TradePower = 10 (suma wszystkich sk³adowych prowincji)
			TradeValue = 20 (suma wszystkich sk³adowych prowincji)
		Podzia³ si³y w wêŸle (bez zewnêtrznych modyfikatorów) bêdzie wygl¹da³ nastepuj¹co:
			X - 7/10 = 70% trade power w wêŸle
			Y - 1/10 = 10% trade power w wêŸle
			Z - 2/10 = 20% trade power w wêŸle
		Oznacza to, ¿e X decyduje o tym co siê dzieje z 70% Value wêz³a, Y decyduje o 10% a Z o 20%.
		Decyzja oznacza jedno z dwóch:
			- Zbiera swoj¹ czêœæ pieni¹dze i zarabia na tym na miejscu
			- Steruje swoj¹ wartoœæ do wybranego wêz³a z puli Outgoing, by dodaæ t¹ wartoœæ do tamtego wêz³a. 

W wêz³ach, kraje mog¹ umieœciæ swoich kupców. Kupiec dodaje trochê TradePower do wêz³a, na korzyœæ kraju, który go tam umieœci³,
oraz pozwala na wybór - czy w wêŸle zbiera pieni¹dze czy przekieruje je dalej w dó³ strumienia.

	Merchant
		+ Country
		+ Action - zbiera (Collect) albo transferuje (Transfer)

---Obliczanie wartoœci w wêŸle---
Proces ten dzielony jest na 2 fazy.
	Faza 1: Podzia³
		Ze stosunku TradePower wszystkich graczy Collectuj¹cych w TradeNode do ca³ego TradePower wêz³a, obliczana jest wartoœæ,
		która zostaje w wêŸle. Pozosta³a wartoœæ, to wartoœæ bêd¹ca przesy³ana dalej do kolejnych wêz³ów.

		TradeNodeValue = 100
		TradePower = 100

		Przyk³adowo:
		U - (20) 20% trade power w wêŸle
		W - (30) 30% trade power w wêŸle
		X -	(10) 10% trade power w wêŸle
		Y -	(15) 15% trade power w wêŸle
		Z - (25) 25% trade power w wêŸle

		U,W,X collect
		Y,Z - transfer

		Po fazie pierwszej w wêŸle zostaje 60 Value, a 40 bêdzie transferowane dalej.

	Faza 2: Zbieranie
		Dzielimy graczy na dwie grupy: Collect i Trasnfering. Teraz podzia³ TradePower odbywa siê miêdzy nimi.
		Ka¿dy gracz z grupy collect oblicza swój udzia³ na podstawie tego jaki ma tradePower w stosunku do innych graczy
		w grupie collect. Czyli:
		Suma TradePower wszystkich collectuj¹cych w wêŸle to 60.
		U posiada wiêc 20/60 czyli 33% TradePower wiêc zbierze on 33% z 60 czyli 20.
		W posiada wiêc 30/60 czyli 50% TradePower wiêc zbierze on 50% z 60 czyli 30.
		X posiada wiêc 10/60 czyli 16% TradePower wiêc zbierze on 16% z 60 czyli 10.
		W grupie transfering z kolei:
		Suma TradePower wszywstkich tranferuj¹cych to 40.
		Y posiada 15/40 czyli 37% TradePower wiêc przetransferuje on 37% z 40 czyli 15.
		Z posiada 25/40 czyli 63% TradePower wiêc przetransferuje on 63% z 40 czyli 25.

Przyk³adowa sytuacja:
	Wêze³ A:
		TradePower - 10 (X posiada 40%, Y posiada 60%) 
		TradeValue - 20
	Wêze³ B:
		TradePower - 10 (X posiada 70%, Y posiada 30%)
		TradeValue - 20
	A -->-->--> B 
	Gracz X decyduje, ¿e w wêŸle A steruje handel do wêz³a B i tam zbiera wartoœæ.
	Gracz Y decyduje, ¿e w obydwu wêz³ach zbiera on pieni¹dze.

	Sytuacja wiêc wygl¹daæ bêdzie nastêpuj¹co:
	Wêze³ A:
		TradePower - 10 (X posiada 40%, Y posiada 60%) 
		TradeValue - 12 (8 jest transferowane do B)
	Wêze³ B:
		TradePower - 10 (X posiada 70%, Y posiada 30%)
		TradeValue - 28 (8 przysz³o z A)
	X zbiera w B 70% z 28 czyli 19.6.
	Y zbiera w A 12 (bo tylko on tam zbiera) i w B 8.4 (bo tutaj dzieli siê pul¹ z X) czyli ³¹cznie 20.4.
	TradePower s³u¿y do podzia³u puli miêdzy graczy a nastêpnie ta pula przek³ada siê na wartoœci pieniê¿ne node'a.

Przyk³adowa sytuacja 2:
	Wêze³ A:
		TradePower - 10 (U posiada 20%, W posiada 20%, X posiada 20%, Y posiada 20%, Z posiada 20%)
		TradeValue - 20
	Wszyscy poza U decyduj¹ siê kierowaæ handel w dó³ strumienia.
	Oznacza to, ¿e w wêŸle A zostanie 4, z którego 100% zbierze U.
	Z wêz³a A przekierowane zostanie 16.