namespace DrawingGame.Backend.Generator;

public static class AnswerGenerator
{

    private static readonly string Words =
        "auto, kočka, pes, strom, dům, slunce, měsíc, hvězda, vlak, autobus, kolo, motorka, letadlo, loď, ryba, žába, had, lev, tygr, medvěd, slon, opice, kráva, prase, ovce, kuře, kachna, myš, zajíc, vlk, liška, ježek, želva, krokodýl, velbloud, žirafa, zebra, tučňák, delfín, velryba, žralok, chobotnice, pizza, hamburger, párek, rohlík, chleba, máslo, sýr, mléko, jogurt, jablko, hruška, banán, pomeranč, citron, jahoda, malina, borůvka, meloun, mrkev, brambora, rajče, okurka, cibule, česnek, paprika, houba, polévka, dort, sušenka, čokoláda, zmrzlina, bonbon, hrnek, talíř, lžíce, vidlička, nůž, postel, stůl, židle, skříň, okno, dveře, koberec, televize, rádio, telefon, počítač, klávesnice, myš, monitor, notebook, nabíječka, baterka, hodiny, budík, kniha, sešit, tužka, pero, pravítko, guma, batoh, škola, nemocnice, obchod, restaurace, kino, divadlo, stadion, bazén, park, most, silnice, semafor, přechod, tunel, hrad, zámek, věž, kostel, socha, fontána, zahrada, květina, růže, tulipán, tráva, list, les, hora, řeka, jezero, moře, pláž, ostrov, déšť, sníh, vítr, bouřka, duha, mrak, blesk, oheň, voda, led, písek, kámen, diamant, koruna, prsten, brýle, čepice, bunda, tričko, kalhoty, ponožka, bota, rukavice, šála, deštník, kufr, balón, míč, raketa, hokejka, brusle, lyže, snowboard, kytara, housle, buben, mikrofon, kamera, fotoaparát, obraz, mapa, vlajka, robot, raketa, astronaut, planeta, sopka, kometa, duch, čaroděj, rytíř, pirát, ninja, drak, princezna, král, koruna, meč, štít, poklad, klíč, zámek, svíčka, dárek, dort, svatba, narozeniny, Vánoce, Velikonoce, sněhulák, anděl, čert, strašidlo, klaun, cirkus, puzzle, šachy, kostka, domino, houpačka, skluzavka, stan, táborák, kompas, dalekohled, lupa, magnet, vrtulník, bagr, traktor, kombajn, sanitka, policie, hasiči";
    
    public static string GenerateAnswer()
    {
        string[] words = Words.Split(", ");
        return words[Random.Shared.Next(words.Length)];
    }
}