using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{

    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    public Text probMessage1;
    public Text probMessage2;

    public int[] values = new int[52];
    int cardIndex = 0;
    int round = 0;

    int[] cardsPlayer = new int[50];
    int[] cardsDealer = new int[50];


    private void Awake()
    {
        InitCardValues();

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */

        int aux = 0;
        for(int i =0;i<values.Length;i++) //recorre el array de valores
        {
            if(aux > 9) //si el auxiliar el mayor de 9(es decir, las cartas son mayores de 9) se le asignan el número 10
            {
                values[i] = 10;
                values[++i] = 10;
                values[++i] = 10;
                aux = 0;
            }
            else{ //si el aux es menor de 9 se le asigna a las cartas los valores correspondientes
                values[i] = aux + 1;
                aux ++;
            }
        }


    

    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */

        int rndNum; // variable para el num random
        Sprite tempFaces;
        int tempValues;

        for (int i = 0; i < 52; i++) // bucle que recorre las 52 cartas
        {
            rndNum = Random.Range(0, 52); // num random

            //SE HACE RANDOM LOS ARRAYS DE FACES
            tempFaces = faces[0]; // tempFaces valdrá lo q valga el primer valor del array faces
            faces[0] = faces[rndNum]; // el valor valdrá un rndnum
            faces[rndNum] = tempFaces; // ese eleemento valdrá tempFaces
            
            //SE HACE RANDOM LOS ARRAYS DE VALUES
            tempValues = values[0]; // tempvalues valdrá el valor de values primera cas
            values[0] = values[rndNum]; // que será un numRandom
            values[rndNum] = tempValues; // y ese valdrá tempValues
        }

    }

    void StartGame()
    {

        for (int i = 0; i < 2; i++) //al empezar el juego se le reparten 2 cartas a cada jugador
        {
            PushDealer();
            PushPlayer();
            round++; //aumentamos la ronda

        }

        //se crea la variable points, que recoge los datos de las manos de cada jugador
        int dealerPoints = dealer.GetComponent<CardHand>().points;
        int playerPoints = player.GetComponent<CardHand>().points;


        /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */



        if (playerPoints == 21) // si el valor de las cartas del jugador vale 21
        {
            finalMessage.text = "Blacjack! Ganaste"; // gana el jugador
            stickButton.interactable = false; // inhabilitamos los dos botones
            hitButton.interactable = false;
        }
        else if (dealerPoints == 21) // si el valor de las cartas del dealer vale 21
        {
            finalMessage.text = "Blacjack! Perdiste"; // pierdes
            stickButton.interactable = false; // inhabilitamos los dos botones
            hitButton.interactable = false;
        }
        if (playerPoints > 21) // si el valor de las cartas del jugador vale más de 21
        {
            finalMessage.text = "Te pasaste, perdiste"; // pierdes
            stickButton.interactable = false; // inhabilitamos los dos botones
            hitButton.interactable = false;
        }
        else if (dealerPoints > 21) // si el valor de las cartas del dealer vale más de 21
        {
            finalMessage.text = "El dealer se pasó, ganaste"; // ganas
            stickButton.interactable = false; // inhabilitamos los dos botones
            hitButton.interactable = false;
        }

    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */


        //se crea la variable points, que recoge los datos de las manos de cada jugador
        int dealerPoints = dealer.GetComponent<CardHand>().points;
        int playerPoints = player.GetComponent<CardHand>().points;

        float probabilidad; // variable float de la probabilidad
        int casosPosibles; // variable de casos posibles

        // Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador

        if (round != 0) // si la ronda es diferente de 0, es decir el juego ha comenzado
        {
            int valoresVisiblesDealer = dealerPoints - cardsDealer[0]; // variable valoresVisDea valdrá valuesDealer menos la posición 1 de sus cartas
            casosPosibles = 13 - playerPoints + valoresVisiblesDealer; // casos posibles será 13 menos valuesPlayes más la anterior variables
            probabilidad = casosPosibles / 13f; // la probabilidad son los caso posibles dividido 13f

            if (probabilidad > 1) // si la probabilidad es mayor que 1
            {
                probabilidad = 1; // valdrá 1 
            }
            else if (probabilidad < 0) // también si es menos que 0
            {
                probabilidad = 0; // valdrá 0
            }
            int diferencia = playerPoints - valoresVisiblesDealer;

            if (diferencia >=10)
            {
                probabilidad = 0;
            }

            probMessage.text = (probabilidad * 100).ToString() + " %"; // el texto de probabilidad será la probabilidad por 100 a string

        }

        //Probabilidad de que el jugador obtenga más de 21 si pide una carta

        float probabilidad2; // variable prob2
        int casosPosibles2; // variable casos posibles 2
        casosPosibles2 = 13 - (21 - playerPoints); // esta valdrá 13 menos (21 menos el valor del jugador)
        probabilidad2 = casosPosibles2 / 13f; // la prob valdrá casos posibles dividido 13f

        if (probabilidad2 > 1) // si la probabilidad es mayor que 1
        {
            probabilidad2 = 1; // valdrá 1 
        }
        else if (probabilidad2 < 0) // también si la probabilidad es menor que 0
        {
            probabilidad2 = 0; // valdrá 0
        }


        if ((21 - playerPoints) > 10)
        {
            probabilidad2 = 0;
        }
        probMessage1.text = (probabilidad2 * 100).ToString() + " %"; // mostramos la probabilidad



        // Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta

        float probabilidadLlegarA17; // variable para la probabilidad de llegar a 17
        int casosPosiblesHasta17; // variable para casps posible
        casosPosiblesHasta17 = 13 - (16 - playerPoints); // casos posibles valdrá 13 menos (16 menos el valuesPlayer)
        probabilidadLlegarA17 = casosPosiblesHasta17 / 13f; // probabilidad de llegar a 17 valdrá los casos partido 13f

        if (probabilidadLlegarA17 > 1) // si la probabilidad es mayor que 1
        {
            probabilidadLlegarA17 = 1; // valdrá 1 
        }
        else if (probabilidadLlegarA17 < 0) // también si la probabilidad es menor que 0
        {
            probabilidadLlegarA17 = 0; // valdrá 0
        }

        if((16 - playerPoints) > 10)
        {
            probabilidadLlegarA17 = 0;
        }


        float probabilidadEntre17y21 = probabilidadLlegarA17 - probabilidad2; // probabilidad entre 17 y 21, prob de llegar a 17 menos la de 21

        if (probabilidadEntre17y21 > 1) // si la probabilidad es mayor que 1
        {
            probabilidadEntre17y21 = 1; // valdrá 1 
        }
        else if (probabilidadEntre17y21 < 0) // también si la probabilidad es menor que 0
        {
            probabilidadEntre17y21 = 0; // valdrá 0
        }

        probMessage2.text = (probabilidadEntre17y21 * 100).ToString() + " %"; // mostramos la probabilidad

    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */


        //se crea la variable points, que recoge los datos de las manos de cada jugador
        int dealerPoints = dealer.GetComponent<CardHand>().points;
        int playerPoints = player.GetComponent<CardHand>().points;

        dealer.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
        dealerPoints = dealerPoints + values[cardIndex]; //los valores del dealer serán los de si mismo más el valor de la nueva carta
        cardsDealer[round] = values[cardIndex];
        cardIndex++;

    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */

         
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);


        //se crea la variable points, que recoge los datos de las manos de cada jugador
        int playerPoints = player.GetComponent<CardHand>().points;
        int dealerPoints = dealer.GetComponent<CardHand>().points;



        playerPoints = playerPoints + values[cardIndex]; //los valores del jugador serán los de si mismo más el valor de la nueva carta
        cardsPlayer[round] = values[cardIndex];

        cardIndex++;
        CalculateProbabilities();


    }

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */



        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */

        //se crea la variable points, que recoge los datos de las manos de cada jugador
        int playerPoints = player.GetComponent<CardHand>().points;


        if (playerPoints > 21) //Si es mayor de 21 pierde
        {
            finalMessage.text = "Perdiste";
            stickButton.interactable = false;
            hitButton.interactable = false;


        }
        else if (playerPoints == 21) //Si es igual a 21 hace blackjack
        {
            finalMessage.text = "Blackjack! Ganaste";
            stickButton.interactable = false;
            hitButton.interactable = false;
        }

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        
        /*TODO:
       * Repartimos cartas al dealer si tiene 16 puntos o menos
       * El dealer se planta al obtener 17 puntos o más
       * Mostramos el mensaje del que ha ganado
       */

       

        //se crea la variable points, que recoge los datos de las manos de cada jugador
        int playerPoints = player.GetComponent<CardHand>().points;
        int dealerPoints = dealer.GetComponent<CardHand>().points;

        while (dealerPoints <= 16) //Mientras que los puntos del dealer sean menos de 16 irá cogiendo cartas (aunque no se muestren en pantalla)
        {
            PushDealer();
            dealerPoints = dealer.GetComponent<CardHand>().points; //sin esto el dealer cogería cartas infinitas
        }
        if (dealerPoints > 21 || dealerPoints < playerPoints) //si el dealer se pasa de 21 o tiene menos puntos que el jugador
        {
            finalMessage.text = "Ganaste"; // ganas
        }
        else if (playerPoints < dealerPoints) //si el dealer tiene más puntos que el jugador
        {
            finalMessage.text = "Perdiste"; // pierdes

        }
        else if (dealerPoints == 21) //si el dealer llega a 21
        {
            finalMessage.text = "Blacjack! Perdiste"; // pierdes

        }
        else if (dealerPoints == playerPoints) //si el jugador llega a 21
        {
            finalMessage.text = "Empataste"; // empatas

        }

    }

    public void PlayAgain()
    {
        //se reactivan los botones
        hitButton.interactable = true;
        stickButton.interactable = true;
        //el mensaje se vacía
        finalMessage.text = "";
        //se vacían las manos
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        //el índice de cartas se pone a 0
        cardIndex = 0;
        
        //la ronda se reinicia
        round = 0;

        //se barajan las cartas y empieza el juego
        ShuffleCards();
        StartGame();
    }

}
