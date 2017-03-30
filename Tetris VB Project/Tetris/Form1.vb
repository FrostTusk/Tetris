Option Explicit On
Option Strict On
Public Class Form1
    'Matrix van 10 breed (0 tot 9) en 20 hoog (0 tot 3)
    Dim rooster(9, 19) As Label
    'GameOver maken we aan als het spel gedaan is.
    Dim GameOver As Label

    'randomTetromino is de willekeurige tetromino die we aanroepen.
    'blokje is de vorm van de tetromino
    'positiex is de meest linkse x coordinaat van het blokje
    'positiey is de kleinste y coordinaat van het blokje. (positiex,positiey) is de linkerbovenhoek
    'vorigex en vorigey zijn de vorige positiex en positiey
    'orientatie is de orientatie van het blokje
    'kleur is de kleur van het blokje.
    Dim randomTetromino As String
    Dim blokje As Boolean(,)
    Dim positiex As Integer
    Dim positiey As Integer
    Dim vorigex As Integer
    Dim vorigey As Integer
    Dim orientatie As Integer
    Dim kleur As Color

    'als itsover True is, is het spel afgelopen
    'als go True is, is blokje pas aangemaakt
    'als wait true is hebben we op pause gedrukt
    Dim itsover As Boolean
    Dim go As Boolean
    Dim wait As Boolean

    'interval staat voor het interval van de timer: MovingDown
    'score is de score op het moment
    'highscore is de highscore van het spel
    'regel is de lijn waar de linkerbovenhoek zich bevindt
    'geschrapt is het aantal lijnen dat zijn geschrapt.
    Dim interval As Integer
    Dim score As Integer
    Dim highScore As Integer
    Dim regel As Integer
    Dim geschrapt As Integer

    'als joker True, hebben we de joker gebruikt.
    'jokerCount is hoe lang de joker al bezig is
    'jokerInterval is het interval dat de timer had voor we de joker hebben gebruikt.
    Dim joker As Boolean
    Dim jokerCount As Integer
    Dim jokerInterval As Integer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.KeyPreview = True
        'Initialisatie van het rooster
        For i As Integer = rooster.GetLowerBound(0) To rooster.GetUpperBound(0)
            For j As Integer = rooster.GetLowerBound(1) To rooster.GetUpperBound(1)
                rooster(i, j) = New Label
                rooster(i, j).Size = New Size(20, 20)
                rooster(i, j).Name = "tile_" & i & "_" & j
                'x,y locatie in het venster
                rooster(i, j).Location = New Point(20 + 21 * i, 20 + 21 * j)
                'kleur van het vierkantje
                rooster(i, j).BackColor = Color.Gray
                'voeg toe aan GUI
                Me.Controls.Add(rooster(i, j))
            Next
        Next
        'Eerst initialiseer ik de eerste score, de highscore op 0
        'Interval wordt geinitialiseerd op 1000, zodat het blokje (de tetromino) 
        'in het begin valt met een snelheid van 1 pixel (1 pixel = 1 plekje in het rooster) per seconde
        score = 0
        highScore = 0
        geschrapt = 0
        interval = 1000
        joker = False
        jokerCount = 0
        'Na alles te initialiseren, start de eerste start sub
        start()
    End Sub
    Private Sub start()
        'Parameters worden geinitialiseerd
        'Go betekent dat het blokje bovenaan start
        'Dit gebruik ik later in mijn programma om te kunnen verwijzen naar het begin van het 'vallen'
        go = True
        regel = 0
        positiey = 0
        orientatie = 0
        'Voordat het spel begint wordt gechecked wat de 'gamestate' is
        'Dit gebeurt dus altijd nadat een blokje niet meer kan vallen (en helemaal in het begin)
        'gameState kijkt na of er lijnen kunnen worden geschrapt en verhoogt daarna de score
        gameState()
        'speeUp versnelt het spel indien nodig
        speedUp()

        'Hier haal ik een nieuw blokje bovenaan.
        'pickRandom zorgt ervoor dat het willekeurig is, getTetromino haalt het op
        'ernaa initialiseer ik nog de nodige parameters, vorigex en vorigey. Dit doe ik zodat ik later juist mijn vorig blokje kan wegwissen.
        'Checkomgeving kijkt de eindconditie na, daarna teken ik het blokje. Ik teken het ook nog als de eindconditie voldaan is zodat de speler weet hoe hij heeft verloren.
        'Hierdoor ontstaat wel het probleem dat de speler het blokje nog steeds kan bewegen op het einde, het valt natuurlijk niet meer
        'maar de speler kan het nog steeds naar links en rechts bewegen en het doen draaien. Dit is niet een groot probleem want de score wordt nooit meer verhooggt en het blokje valt niet meer.
        pickRandomTetromino()
        getTetromino()
        vorigex = positiex
        vorigey = positiey
        checkOmgeving(1)
        tekenBlokje()

        'De timer: MovingDown, start enkel als itsover not true is, dus als het spel niet gedaan is
        If itsover <> True Then
            MovingDown.Start()
        End If
        'Dit zorgt ervoor dat helemaal in het begin, de highscore steeds de current score is.
        If highScore = 0 Then
            OpHighscore.Text = "Highscore: " + CType(score, String)
        End If
    End Sub
    Private Sub full()
        'Deze sub, full(), wordt aangeroepen helemaal op het eind van het spel.
        'fnt is het font van de nieuwe label, gameover. itsover is een boolean die zegt of het spel gedaan is of niet.
        Dim fnt As Font
        itsover = True

        'Hier maak ik een nieuw label, namelijk GameOver zodat ik een gameover melding aan de speler kan geven
        GameOver = New Label
        GameOver.Size = New Size(150, 25)
        GameOver.Location = New Point(50, 100)
        GameOver.BackColor = Color.White
        GameOver.ForeColor = Color.Gray
        Me.Controls.Add(GameOver)

        GameOver.BringToFront()
        fnt = GameOver.Font
        GameOver.Font = New Font(fnt.Name, 12, FontStyle.Bold)
        GameOver.Text = "   Game Over!"

        'Highscore wordt opgehoogd als het hoger is dan de vorige highscore
        'Daarna wordt het ook in de display verandert, helemaal in het begin moeten we de highscore ook altijd aan passen.
        If highScore = 0 Then
            highScore = score
        End If
        If score > highScore Then
            highScore = score
            OpHighscore.Text = "Highscore: " + CType(score, String)
        End If
    End Sub

    Private Sub gameState()
        'Deze sub kijkt na of er lijnen moeten worden geschrapt.
        'emptyspace zegt dat een lijn niet volledig vol is want er is een 'emptyspace'.
        'maxx en maxy staan voor de maxima van het rooster
        'scorefactor wilt zeggen hoeveel lijnen we in een keer hebben geschrapt en wat dat betekent voor de score.
        Dim emptyspace As Boolean
        Dim maxx = rooster.GetUpperBound(0)
        Dim maxy = rooster.GetUpperBound(1)
        Dim scorefactor = 0

        'Deze for lus kijkt effectief na of er lijnen kunnen worden geschrapt.
        For y As Integer = 0 To maxy
            'We kijken eerst na of het klopt dat er geen emptyspace is
            emptyspace = False
            For x As Integer = 0 To maxx
                'Indien er wel een emptyspace is, veranderen we dus emptyspace naar true, er is een emptyspace.
                If rooster(x, y).BackColor = Color.Gray Then
                    emptyspace = True
                End If
            Next
            'Als er geen emptyspace is (de lijn is vol) wordt dit aangeroepen.
            'We moeten de lijn schrappen en de scorefactor met 1 ophogen want gaan een lijn schrappen.
            'De y coordinaat van de lijn wordt meegegeven.
            If emptyspace = False Then
                eliminateLine(y)
                scorefactor += 1
            End If
        Next
        'Hier updaten we de score naar de nieuwe score, de scorefactor (met wat we score gaan verhogen, wordt meegegeven als parameter)
        scoreUpdate(scorefactor)
    End Sub
    Private Sub eliminateLine(schrapy As Integer)
        'maxx en maxy ophalen als variabelen.
        'schrapy is de y coordinaat van de lijn die geschrapt moet worden
        Dim maxx = rooster.GetUpperBound(0)
        Dim maxy = rooster.GetUpperBound(1)

        'Hier schrap ik de lijn.
        For x As Integer = 0 To maxx
            rooster(x, schrapy).BackColor = Color.Gray
        Next

        'Deze for lus dient om alles boven de geschrapte lijn, te verlagen naar een lijn onder zijn oorspronkelijke positie.
        'Dit doen we omdat we een lijn 'weg doen'.
        For y As Integer = schrapy - 1 To 0 Step -1
            For x As Integer = 0 To maxx
                kleur = rooster(x, y).BackColor
                rooster(x, y).BackColor = Color.Gray
                rooster(x, y + 1).BackColor = kleur
            Next
        Next
    End Sub
    Private Sub scoreUpdate(scorefactor As Integer)
        'Hier verhoog ik de score met de scorefactor die heb meegegeven in gameState.
        Select Case scorefactor
            Case 1
                geschrapt += 1
                score += 40
                OpScore.Text = "Score: " + CType(score, String)
            Case 2
                geschrapt += 2
                score += 100
                OpScore.Text = "Score: " + CType(score, String)
            Case 3
                geschrapt += 3
                score += 300
                OpScore.Text = "Score: " + CType(score, String)
            Case 4
                geschrapt += 4
                score += 1200
                OpScore.Text = "Score: " + CType(score, String)
        End Select
    End Sub
    Private Sub speedUp()
        'Hier verhoog ik de snelheid van het spel indien dit nodig is.
        'geschrapt is een globale variable die we bijhouden om bij te houden hoeveel lijnen we al hebben geschrapt.
        If geschrapt = 5 Then
            interval = 750
            MovingDown.Interval = interval
        ElseIf geschrapt = 10 Then
            interval = 500
            MovingDown.Interval = interval
        ElseIf geschrapt = 15 Then
            interval = 300
            MovingDown.Interval = interval
        End If
    End Sub

    Private Sub pickRandomTetromino()
        'Deze sub gebruiken we om een willekeurige tetromino te kiezen uit de lijst tetrominos.
        Dim rnd = New Random()
        Dim tetrominos = {"square", "straight", "T", "J", "L", "S", "Z"}
        randomTetromino = tetrominos(rnd.Next(0, tetrominos.Count))
    End Sub
    Private Sub getTetromino()
        'Deze sub wordt gebruikt om de tetromino op te halen. Deze sub gaan we steeds aanroepen als er iets verandert aan de tetromino.
        Select Case randomTetromino
            Case "square"
                squareTetromino(orientatie)
            Case "straight"
                straightTetromino(orientatie)
            Case "T"
                T_Tetromino(orientatie)
            Case "J"
                J_Tetromino(orientatie)
            Case "L"
                L_Tetromino(orientatie)
            Case "S"
                S_Tetromino(orientatie)
            Case "Z"
                Z_Tetromino(orientatie)
        End Select
    End Sub

    'De volgende subs zijn allemaal definities van een specifieke Tetromino. Ze worden voorgesteld als boolean zodat we kunnen werken met een relatieve x en y coordinaat.
    'Ze hebben allemaal de parameter, orientatie, nodig. In de funcite wordt dit voorgesteld als n.
    'Ze gebruiken allemaal een if statement die checked of go = true. Dit wil zeggen dat het blokje pas gespawned is. Dan geven we de juiste x en y posities mee.
    'straightTetromino is de enige definitie die we een beetje meer aangepast, dit komt omdat we steeds draaien rond de linkerboven hoek. 
    '(Op het practicum blad Is er niet aangegeven rond welk punt we moeten draaien)
    'Maar bij straight is het aangepast zodat het spel vloeiender speelt met een straightTetromino.
    Private Sub squareTetromino(n As Integer)
        orientatie = n
        Select Case n
            Case 0, 1, 2, 3
                blokje = {{True, True}, {True, True}}
        End Select

        If go = True Then
            positiey = 0
            positiex = 4
        End If

        kleur = Color.Yellow
    End Sub
    Private Sub straightTetromino(n As Integer)
        orientatie = n
        'Zoals je ziet passen we hier de x en y coordinaat aan van het plakje, zodat we simuleren dat het blokje rond een ander punt draait.
        Select Case n
            Case 0, 2
                blokje = {{True}, {True}, {True}, {True}}
            Case 1, 3
                blokje = {{True, True, True, True}}
        End Select

        If go = True Then
            positiey = 0
            positiex = 3
        End If

        kleur = Color.LightBlue
    End Sub
    Private Sub T_Tetromino(n As Integer)
        orientatie = n
        Select Case n
            Case 0
                blokje = {{False, True}, {True, True}, {False, True}}
            Case 1
                blokje = {{True, True, True}, {False, True, False}}
            Case 2
                blokje = {{True, False}, {True, True}, {True, False}}
            Case 3
                blokje = {{False, True, False}, {True, True, True}}
        End Select

        If go = True Then
            positiey = 0
            positiex = 4
        End If

        kleur = Color.Purple
    End Sub
    Private Sub J_Tetromino(n As Integer)
        orientatie = n
        Select Case n
            Case 0
                blokje = {{True, True}, {False, True}, {False, True}}
            Case 1
                blokje = {{True, True, True}, {True, False, False}}
            Case 2
                blokje = {{True, False}, {True, False}, {True, True}}
            Case 3
                blokje = {{False, False, True}, {True, True, True}}
        End Select

        If go = True Then
            positiey = 0
            positiex = 4
        End If

        kleur = Color.Blue
    End Sub
    Private Sub L_Tetromino(n As Integer)
        orientatie = n
        Select Case n
            Case 0
                blokje = {{False, True}, {False, True}, {True, True}}
            Case 1
                blokje = {{True, True, True}, {False, False, True}}
            Case 2
                blokje = {{True, True}, {True, False}, {True, False}}
            Case 3
                blokje = {{True, False, False}, {True, True, True}}
        End Select

        If go = True Then
            positiey = 0
            positiex = 4
        End If

        kleur = Color.Orange
    End Sub
    Private Sub S_Tetromino(n As Integer)
        orientatie = n
        Select Case n
            Case 0, 2
                blokje = {{False, True}, {True, True}, {True, False}}
            Case 1, 3
                blokje = {{True, True, False}, {False, True, True}}
        End Select

        If go = True Then
            positiey = 0
            positiex = 4
        End If

        kleur = Color.LightGreen
    End Sub
    Private Sub Z_Tetromino(n As Integer)
        orientatie = n
        Select Case n
            Case 0, 2
                blokje = {{True, False}, {True, True}, {False, True}}
            Case 1, 3
                blokje = {{False, True, True}, {True, True, False}}
        End Select
        If go = True Then
            positiey = 0
            positiex = 4
        End If

        kleur = Color.Red
    End Sub

    Private Sub tekenBlokje()
        'Met deze sub teken we het blokje in het rooster
        'We doen dit het rooster in te kleuren op de plaats waar het blokje zich bevindt.
        'Omdat we relatieve coordinaten gebruiken kunnen we dit doen met 2 'eenvoudige' for loops
        'eindx en eindy zijn de absolute x en y eindwaarden van het blokje.
        Dim eindx = positiex + blokje.GetUpperBound(0)
        Dim eindy = positiey + blokje.GetUpperBound(1)
        For x As Integer = positiex To eindx
            For y As Integer = positiey To eindy
                If blokje(x - positiex, y - positiey) = True Then
                    rooster(x, y).BackColor = kleur
                End If
            Next
        Next
    End Sub
    Private Sub eraseBlokje()
        'Deze sub is bijna net dezelfde als tekenBlokje.
        'Het verschil is dat we nu de vorige x en y linkerboven hoek gaan gebruiken, zodat we het vorige blokje kunnen wissen.
        'Dit doen we door het rooster met grijs op die plaats in te kleuren.
        Dim eindx = vorigex + blokje.GetUpperBound(0)
        Dim eindy = vorigey + blokje.GetUpperBound(1)
        For x As Integer = vorigex To eindx
            For y As Integer = vorigey To eindy
                If blokje(x - vorigex, y - vorigey) = True Then
                    rooster(x, y).BackColor = Color.Gray
                End If
            Next
        Next
    End Sub
    Private Sub checkOmgeving(n As Integer)
        'Deze sub neemt 1 parameter mee, deze parameter bepaalt of de sub de omgeving op zichzelf nakijkt of diegene onder hem.
        'checkOmgeving(0) gebruiken we wanner het blokje valt, checkOmgeving(1) gebruiken we wanneer een nieuw blokje valt of wanneer het blokje wordt geroteerd.
        'erisplaats betekent dat er plaats is op of onder het blokje.
        Dim erisplaats As Boolean
        Dim eindx = positiex + blokje.GetUpperBound(0)
        Dim eindy = positiey + blokje.GetUpperBound(1)
        vorigex = positiex
        vorigey = positiey

        Select Case n
            Case 0
                'Deze case kijkt de omgeving onder het blokje na.
                erisplaats = True
                For x As Integer = positiex To eindx
                    For y As Integer = positiey To eindy
                        If blokje(x - vorigex, y - vorigey) = True Then
                            'als in het blokje de pixel true is
                            If y + 1 > eindy And eindy < rooster.GetUpperBound(1) Then
                                'als de volgende y coordinaat buiten de bounds van het blokje ligt maar niet buiten het rooster
                                If rooster(x, y + 1).BackColor <> Color.Gray Then
                                    'als deze plaats niet grijs is (het is ingekleurd)
                                    erisplaats = False
                                    'dan is er geen plaats.
                                End If
                            End If
                            If y + 1 <= eindy Then
                                'als de volgende y coordinaat in het blokje ligt
                                If blokje(x - positiex, y - positiey + 1) = False Then
                                    'als de volgende pixel false is
                                    If rooster(x, y + 1).BackColor <> Color.Gray Then
                                        'als deze plaats in het rooster ingekleurd is
                                        erisplaats = False
                                        'dan is er geen plaats.
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next
                If go = True And erisplaats = False Then
                    'Indien we pas zijn vertrokken en er geen plaats is, dan is het rooster 'full' we kunnen niet verder spelen
                    full()
                ElseIf erisplaats = False Then
                    'Indien er geen plaats meer is onder het blokje, start een nieuw blokje.
                    MovingDown.Stop()
                    start()
                End If

            Case 1
                'Deze case kijkt de omgeving op het blokje na.
                erisplaats = True
                For x As Integer = positiex To eindx
                    For y As Integer = positiey To eindy
                        If blokje(x - positiex, y - positiey) = True Then
                            'als de pixel in het blokje true is
                            If rooster(x, y).BackColor <> Color.Gray Then
                                'en op die plaats in het rooster is er een kleur
                                erisplaats = False
                                'dan is er geen plaats.
                            End If
                        End If
                    Next
                Next
                If go = True And erisplaats = False Then
                    'Indien we pas zijn vertrokken en er geen plaats is, dan is het rooster 'full' we kunnen niet verder spelen
                    full()
                ElseIf erisplaats = False Then
                    'Indien er geen plaats is, ga terug naar de vorige orientatie
                    If orientatie = 0 Then
                        orientatie = 3
                    Else
                        orientatie -= 1
                    End If
                End If
        End Select
    End Sub

    Private Sub moveDown(sender As Object, e As EventArgs) Handles Me.Load, MovingDown.Tick
        'Eerst checken we de omgeving onde rhet blokje.
        'Als er geen plaats is wordt de timer gestopt.
        checkOmgeving(0)
        'De tekst die zegt op welke regel de linkerbovenhoek zich bevindt wordt ook aangepast.
        OpRegel.Text = "Regel: " + CType(regel, String) + " : " + CType(geschrapt, String)
        ' Voeg dit bij bij de opRegel bewerking om te zijn hoeveel lijnen er zijn geschrapt:
        regel += 1
        'We slaan de vorige x positie op.
        vorigex = positiex
        Dim eindy = positiey + blokje.GetUpperBound(1)
        Dim maxy = rooster.GetUpperBound(1)

        If eindy < maxy Then
            If positiey = 0 And go = True Then
                'als we zijn gestart gebeurt er nog niets
                go = False
            Else
                'Als we al zijn gestart wordt het vorige weggeveeft, de vorige y coordinaat wordt opgehoogd met 1
                'Het blokje wordt getekend en de vorige y coordinaat wordt de huidge.
                eraseBlokje()
                positiey = vorigey + 1
                tekenBlokje()
                vorigey = positiey
            End If
        ElseIf eindy = maxy Then
            'Als we op het eind van het rooster geraken, stopt de timer.
            MovingDown.Stop()
            start()
        End If
    End Sub
    Private Sub moveRight()
        'Deze sub zorgt voor de beweging naar rechts.
        'correct staat voor, we hebben al gecorrigeerd.
        Dim correct = False
        vorigex = positiex
        vorigey = positiey
        Dim eindx = vorigex + blokje.GetUpperBound(0)
        Dim eindy = vorigey + blokje.GetUpperBound(1)
        'We bewegen het blokje met 1 naar rechts (positief)
        positiex = vorigex + 1

        'Indien we uit de bounds gaan, moeten we de positie naar links doen.
        While positiex + blokje.GetUpperBound(0) > rooster.GetUpperBound(0)
            positiex -= 1
        End While

        'Deze for lus kijkt na of er iets rechts van het blokje ligt.
        For x As Integer = vorigex To eindx
            For y As Integer = vorigey To eindy
                'Indien de pixel in het blokje true is
                If blokje(x - vorigex, y - vorigey) = True And correct = False Then
                    'Indien de volgende x buiten het blokje ligt, en eindx kleiner als het maximum van het rooster is, en je hebt nog niet gecorrigeerd.
                    If x + 1 > eindx And eindx < rooster.GetUpperBound(0) Then
                        'Als het rooster op die plaats al ingekleurd is
                        If rooster(x + 1, y).BackColor <> Color.Gray Then
                            correct = True
                            'moet je corrigeren
                        End If
                    End If
                    'Indien de pixel in bounds ligt, en je hebt nog niet gecorrigeerd.
                    If x + 1 <= eindx And correct = False Then
                        'Indien de volgende pixel false is.
                        If blokje(x - vorigex + 1, y - vorigey) = False Then
                            'Als het rooster op die plaats al ingekleurd is
                            If rooster(x + 1, y).BackColor <> Color.Gray Then
                                correct = True
                                'moet je corrigeren
                            End If
                        End If
                    End If
                End If
            Next
        Next

        'Als je moet corrigeren dan wordt de positie -1 gedaan.
        If correct = True Then
            positiex -= 1
        End If
        'Doe het vorige weg, teken het nieuwe.
        eraseBlokje()
        tekenBlokje()
    End Sub
    Private Sub moveLeft()
        'Deze sub zorgt voor de beweging naar links.
        'correct staat voor, we hebben al gecorrigeerd.
        Dim correct = False
        vorigex = positiex
        vorigey = positiey
        Dim eindx = vorigex + blokje.GetUpperBound(0)
        Dim eindy = vorigey + blokje.GetUpperBound(1)
        positiex = vorigex - 1

        'Indien we uit de bounds gaan, moeten we de positie naar rechts doen.
        While positiex < 0
            positiex += 1
        End While

        'Deze for lus kijkt na of er iets links van het blokje ligt.
        For x As Integer = eindx To vorigex Step -1
            For y As Integer = vorigey To eindy
                'Indien de pixel in het blokje true is
                If blokje((x + blokje.GetUpperBound(0)) - eindx, y - vorigey) = True Then
                    'Indien de volgende x buiten het blokje ligt, en eindx groter als het minimum van het rooster is, en je hebt nog niet gecorrigeerd.
                    If x - 1 < vorigex And vorigex > 0 And correct = False Then
                        'Als het rooster op die plaats al ingekleurd is
                        If rooster(x - 1, y).BackColor <> Color.Gray Then
                            correct = True
                            'moet je corrigeren
                        End If
                    End If
                    'Indien de pixel in bounds ligt, en je hebt nog niet gecorrigeerd.
                    If x - 1 >= vorigex And correct = False Then
                        'Indien de volgende pixel false is.
                        If blokje((x - eindx) + blokje.GetUpperBound(0) - 1, y - vorigey) = False Then
                            'Als het rooster op die plaats al ingekleurd is
                            If rooster(x - 1, y).BackColor <> Color.Gray Then
                                correct = True
                                'moet je corrigeren
                            End If
                        End If
                    End If
                End If
            Next
        Next

        'Als je moet corrigeren dan wordt de positie +1 gedaan.
        If correct = True Then
            positiex += 1
        End If
        'Doe het vorige weg, teken het nieuwe.
        eraseBlokje()
        tekenBlokje()
    End Sub
    Private Sub rotate()
        Dim maxx = rooster.GetUpperBound(0)
        Dim maxy = rooster.GetUpperBound(1)
        'De vorige orientatie wordt opgeslagen.
        Dim vorigeorientatie = orientatie
        'Eerst wordt het vorige blokje weggedaan
        Dim lastx = positiex
        'De laatste x positie wordt opgeslagen
        vorigex = positiex
        vorigey = positiey
        eraseBlokje()

        'Orientatie wordt cyclisch doorgegeven
        'Orientatie is een globale variabele dus als het hier verandert, verandert het overal.
        If orientatie = 3 Then
            orientatie = 0
        Else
            orientatie += 1
        End If
        'Het nieuwe blokje wordt aangeroepen
        getTetromino()

        'Indien het nieuwe blokje niet helemaal past wordt het aangepast. 
        While positiex + blokje.GetUpperBound(0) > maxx
            'als x coordinaat te groot is verkleinen we het.
            positiex -= 1
        End While

        If positiey + blokje.GetUpperBound(1) > maxy Then
            'Indien we door het maximum van het rooster draaien gaan we terug naar de vorige orienatie en herstellen we de vorige x coordinaat.
            orientatie = vorigeorientatie
            positiex = lastx
        Else
            'Check omgeving rond het nieuwe blokje.
            checkOmgeving(1)
            'Indien er geen plaats is gaan we terug naar de vorige orienatie en herstellen we de vorige x coordinaat.
            If orientatie = vorigeorientatie Then
                positiex = lastx
            End If
        End If
        'We draaien nooit door de bovenkant of de linkerkant van het rooster dus die bounds worden niet gechecked.


        'Het gecorrigeerde blokje wordt aangeroepen.
        getTetromino()
        vorigex = positiex
        vorigey = positiey

        'Het nieuwe blokje wordt getekend
        tekenBlokje()
    End Sub

    Private Sub Form1_KeyDown(ByVal sender As Object,
    ByVal e As KeyEventArgs) Handles Me.KeyDown
        'Ik persoonlijk vind dat Tetris gemakkelijker speelt als je met keydown speelt in de plaats van key up.
        'Indien je liever met key up speelt moet je gewoon de cases right, left en up wegdoen en in keyup. Deze case uit commentaar halen. (Ik heb ze daar 'ook' staan)
        If wait <> True And itsover <> True Then
            Select Case e.KeyCode
                Case Keys.Right
                    'Rechts 
                    moveRight()

                Case Keys.Left
                    'Links
                    moveLeft()

                Case Keys.Up
                    'Omhoog
                    rotate()

                Case Keys.Down
                'Omlaag

                Case Keys.Space
                    'Dit doet het blokje versnellen, als je het indrukt.
                    MovingDown.Interval = 100
            End Select
        End If
    End Sub
    Private Sub Form1_KeyUp(ByVal sender As Object,
    ByVal e As KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            'Deze cases kan je uit comments halen om keyUp als handle te nemen
            'Case Keys.Right
                'Rechts 
            'moveRight()

            'Case Keys.Left
                'Links
            'moveLeft()

            'Case Keys.Up
                'Omhoog
            'rotate()

            'Case Keys.Down
                'Omlaag

            Case Keys.Space
                'Dit doet de snelheid van het blokje reverten naar de snelheid die hij had voordat je spatie hebt ingedrukt.
                MovingDown.Interval = interval
        End Select
    End Sub
    Private Sub PauseKnop_Click(sender As Object, e As EventArgs) Handles PauseKnop.Click
        'Pause stopt het spel in zijn current state. Je kan spijtig genoeg nog wel draaien en bewegen maar ik had niet genoeg tijd meer om dit aan te passen.
        'wait is een boolean variabele die zegt dat het spel zit te wachten op de user om op continue te duwen.
        If wait = True Then
            MovingDown.Start()
            If joker = True Then
                JokerTimer.Start()
            End If
            wait = False
            PauseKnop.Text = "* Pause *"
        Else
            MovingDown.Stop()
            If joker = True Then
                JokerTimer.Stop()
            End If
            wait = True
            PauseKnop.Text = "* Continue *"
        End If
    End Sub
    Private Sub Restart_Click(sender As Object, e As EventArgs) Handles RestartKnop.Click
        Dim maxx = rooster.GetUpperBound(0)
        Dim maxy = rooster.GetUpperBound(1)

        For x As Integer = 0 To maxx
            For y As Integer = 0 To maxy
                rooster(x, y).BackColor = Color.Gray
            Next
        Next
        OpScore.Text = "Score: 0"
        OpRegel.Text = "Regel: 0"
        wait = False
        PauseKnop.Text = "* Pause *"

        If itsover <> True Then
            full()
        End If
        itsover = False

        GameOver.Text = ""
        GameOver.BackColor = Color.Black
        GameOver.ForeColor = Color.Black
        GameOver.SendToBack()

        score = 0
        geschrapt = 0
        interval = 1000
        MovingDown.Interval = interval
        JokerTimer.Stop()
        joker = False
        JokerKnop.Text = "* Joker *"
        start()
    End Sub
    Private Sub JokerKnop_Click(sender As Object, e As EventArgs) Handles JokerKnop.Click
        'Dit is de sub voor de joker
        'Als de joker nog niet gebruikt is geweest in dit spelletje dan mag de joker worden gebruikt.
        If joker = False Then
            'joker wordt true, hij is gebruikt. De tekst verandert naar using, en we starten de jokertimer.
            joker = True
            JokerKnop.Text = "* Using *"
            JokerTimer.Start()

            'jokerCount, hoe lang we joker al aan het gebruiken zijn wordt geinitialiseerd op 0.
            'Het jokerInterval (het interval voor de joker werdt gebruikt) wordt opgeslagen
            'Het MovingDown interval wordt op 1000, de traagste snelheid gezet.
            jokerCount = 0
            jokerInterval = interval
            interval = 1000
            MovingDown.Interval = interval
        End If
    End Sub
    Private Sub JokerTimer_Tick(sender As Object, e As EventArgs) Handles JokerTimer.Tick
        'Elke keer dat joker tikt (elke seconde), verhogen we jokerCount op met 1 en veranderen we de tekst.
        jokerCount += 1
        JokerKnop.Text = CType(jokerCount, String) + " secs"
        If jokerCount = 30 Then
            'Als de joker 30 keer heeft getikt, stop hij, herstelt hij het vorig MovingDown interval en wordt de tekst verandert naar used.
            JokerTimer.Stop()
            interval = jokerInterval
            MovingDown.Interval = interval
            JokerKnop.Text = "* Used *"
        End If
    End Sub
End Class