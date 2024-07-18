namespace BurgerGame
{
    internal class Program
    {
        struct GameData
        {
            #region 맵, 플레이어 관련
            public Point playerPoint;
            public Point playerView;
            public char[,] map;
            public Point doteArt;
            #endregion
            #region 캐릭터 능력치 관련
            public int level;
            public int money;
            public int getMoney;
            public int popularity;
            public int getPopularity;
            public int maxExp;
            public int exp;
            public Player cookSpeed;
            public Player quality;
            public string burgurName;
            #endregion
            #region 스토브 관련
            public Point foodMakeBar;
            public int stoveCount;
            public int maxStoveCount;
            public bool isStove;
            public int burgurCount;
            public int maxBurgurCount;
            #endregion
            #region 상점 관련
            public bool isShop;
            public Point ShopChoicePoint;
            public ShopItemName ShopMyChoice;
            public ShopItem[] shopItem;
            public int popularUpgrade;
            #endregion
            #region 손님관련
            public Guest[] guest;
            public int waitingGuest;
            public int guestCount;
            public Point gusetWaitCountPoint;
            #endregion
            #region 기타
            public bool running;
            public ConsoleKey lastKey;
            public int moveCount;
            public Point explanationKey;
            #endregion
        }
        enum ShopItemName { FastCook, StorageUp, QualityUp, CleanUp, GetPopularUp, SIZE }
        enum GuestName { empty, 험상궂은손님, 많이먹는손님, 친절한손님, 진상손님,SIZE }
        struct Guest
        {
            public GuestName name;
            public bool isGuest;
            public int waitNumber;
            public int waitingTime;
        }
        struct Player
        {
            public int quality;
            public int cookSpeed;
        }
        struct ShopItem
        {
            public int ID;
            public ShopItemName name;
            public string explanation;
            public int price;
        }
        struct Point()
        {
            public int x;
            public int y;
        }
        static GameData data;
        static Random random = new Random();
        static void Main(string[] args)
        {
            Start();
            while (data.running)
            {
                Render();
                Input();
                Update();
            }
            End();
        }
        static void Start()
        {
            #region 시작화면
            Console.Clear();
            Console.WriteLine("■■■■■■■■■■■■■■■■■■■■");
            Console.WriteLine("■                                    ■");
            Console.WriteLine("■               햄버거               ■");
            Console.WriteLine("■                                    ■");
            Console.WriteLine("■■■■■■■■■■■■■■■■■■■■");
            Console.WriteLine("\n 시작하시려면 아무 키나 눌러주세요");
            Console.ReadKey();
            #endregion
            #region 게임 안내 메세지
            Console.Clear();
            Console.WriteLine("햄버거를 만들어서 손님에게 팝시다!");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("♨");
            Console.ResetColor();
            Console.WriteLine("에서 햄버거를 만들 수 있습니다");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("▼");
            Console.ResetColor();
            Console.WriteLine("에서 햄버거를 손님에게 팔 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("상점에서 업그레이드를 할 수 있습니다");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("☎");
            Console.ResetColor();
            Console.WriteLine(": 상점");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("손님에게 햄버거를 팔면 돈과 평판이 오릅니다");
            Console.WriteLine();
            Console.WriteLine("손님이 기다리다 떠나면 평판이 떨어집니다");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("평판이 모두 떨어지면 게임에서 패배합니다!");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\n 이해하셨다면 아무 키나 눌러주세요");
            Console.ReadKey();
            #endregion
            Console.CursorVisible = false;
            #region 맵, 플레이어 관련
            data.map = new char[5, 5]
            {
                {'■','■','♨','■','■'},
                {'■','□','□','□','■'},
                {'■','□','□','□','☎'},
                {'■','□','□','□','■'},
                {'■','■','▼','■','■'},
            };
            data.playerPoint = new Point() { x = 2, y = 2 };
            data.playerView = new Point() { x = data.playerPoint.x, y = data.playerPoint.y + 1 };
            data.doteArt = new Point() { x = 55, y = 0 };
            #endregion
            #region 능력치 및 캐릭터 스텟 관련
            data.money = 5000;
            data.getMoney = 50;
            data.popularity = 300;
            data.getPopularity = 50;
            data.level = 1;
            data.exp = 0;
            data.maxExp = 10;
            data.cookSpeed.cookSpeed = 1;
            data.burgurName = "햄버거";
            data.quality.quality = 0;
            #endregion
            #region 스토브 관련
            data.foodMakeBar = new Point() { x = 6, y = 1 };
            data.stoveCount = 0;
            data.maxStoveCount = 5;
            data.isStove = false;
            data.burgurCount = 0;
            data.maxBurgurCount = 2;
            #endregion
            #region 상점 관련
            data.isShop = false;
            data.ShopChoicePoint = new Point() { x = 1, y = 12 };
            data.ShopMyChoice = ShopItemName.FastCook;
            data.shopItem = new ShopItem[5];
            data.popularUpgrade = 15;
            #region 상점 아이템 설정       
            #region FastCook 업그레이드
            data.shopItem[0].ID = 0;
            data.shopItem[0].name = ShopItemName.FastCook;
            data.shopItem[0].explanation = "패티를 굽는 데 걸리는 시간이 감소합니다";
            data.shopItem[0].price = 3000;
            #endregion
            #region StorageUp 업그레이드
            data.shopItem[1].ID = 1;
            data.shopItem[1].name = ShopItemName.StorageUp;
            data.shopItem[1].explanation = "소지 가능한 햄버거의 양이 늘어납니다";
            data.shopItem[1].price = 1000;
            #endregion
            #region QualityUp 업그레이드
            data.shopItem[2].ID = 2;
            data.shopItem[2].name = ShopItemName.QualityUp;
            data.shopItem[2].explanation = "버거를 더 맛있게 만듭니다";
            data.shopItem[2].price = 2000;
            #endregion
            #region CleanUp 업그레이드
            data.shopItem[3].ID = 3;
            data.shopItem[3].name = ShopItemName.CleanUp;
            data.shopItem[3].explanation = "가게가 더욱 깨끗해집니다 (기능 없음)";
            data.shopItem[3].price = 1000;
            #endregion
            #region GetPopularUp 업그레이드
            data.shopItem[4].ID = 4;
            data.shopItem[4].name = ShopItemName.GetPopularUp;
            data.shopItem[4].explanation = "평판이 더 많이 증가합니다";
            data.shopItem[4].price = 1500;
            #endregion
            #endregion
            #endregion
            #region 손님 관련
            data.guest = new Guest[15];
            for (int i = 0; i < data.guest.Length; i++)
            {
                data.guest[i].isGuest = false;
                data.guest[i].name = GuestName.empty;
                data.guest[i].waitNumber = i + 1;
                data.guest[i].waitingTime = 0;
            }
            data.waitingGuest = 1;
            data.guestCount = 0;
            data.gusetWaitCountPoint = new Point() { x = 30, y = 10 };
            #endregion
            #region 기타 변수
            data.running = true;
            data.lastKey = ConsoleKey.DownArrow;
            data.moveCount = 0;
            data.explanationKey = new Point() { x = 34, y = 0 };
            #endregion
        }
        static void End()
        {
            Console.Clear();
            Console.WriteLine("■■■■■■■■■■■■■■■■■■■■");
            Console.WriteLine("■                                    ■");
            Console.WriteLine("■           파산하였습니다           ■");
            Console.WriteLine("■                                    ■");
            Console.WriteLine("■■■■■■■■■■■■■■■■■■■■");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"받은 손님 수: {data.guestCount}");
            Console.ReadLine();
        }
        static void Render()
        {
            //map
            Console.Clear();
            PrintMap();
            //cookProgree
            ExplainKey();
            PrintMakeBurger();
            //level, money, popularity
            PrintStatus();
            //주문 창
            PrintTextInterface();
        }
        static void Input()
        {
            if (data.isStove)
            {
                InputKeyStove();
                return;
            }
            if (data.isShop)
            {
                InputKeyShop();
                return;
            }
            InputKey();
        }
        static void Update()
        {
            CreateGuest();
            WaitTimeGuest();
            levelUp();
            UpgradeBurger();
            LoseGame();
        }

        #region Render 관련
        #region 좌상단 화면
        static void PrintMap()
        {
            for (int y = 0; y < data.map.GetLength(0); y++)
            {
                for (int x = 0; x < data.map.GetLength(1); x++)
                {
                    switch (data.map[y, x])
                    {
                        case '▼':
                        case '☎':
                        case '♨':
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        default:
                            break;
                    }
                    Console.Write(data.map[y, x]);
                    Console.ResetColor();

                }
                Console.WriteLine();
            }
            PrintPlayer();
        }
        static void PrintPlayer()
        {
            Console.SetCursorPosition(((data.playerPoint.x) * 2), data.playerPoint.y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("◆");
            Console.ResetColor();
        }
        #endregion
        #region 우상단 화면
        static void PrintMakeBurger()
        {
            if (data.isStove)
            {
                Console.SetCursorPosition((data.foodMakeBar.x) * 2, data.foodMakeBar.y);
                Console.WriteLine("패티 굽는 중");
                Console.SetCursorPosition(((data.foodMakeBar.x) * 2) + 1, (data.foodMakeBar.y) + 1);
                for (int i = 0; i < 5; i++)
                {
                    if (i <= data.stoveCount - 1)
                        Console.Write("■");
                    else
                        Console.Write("□");
                }
                Console.WriteLine();
            }
        }
        static void ExplainKey()
        {
            Console.SetCursorPosition(data.explanationKey.x, data.explanationKey.y);
            Console.WriteLine(" ← ↑ ↓ → : 이동");
            Console.SetCursorPosition(data.explanationKey.x, data.explanationKey.y + 1);
            Console.WriteLine(" Z: 상호작용");
            Console.SetCursorPosition(data.explanationKey.x, data.explanationKey.y + 2);
            Console.WriteLine(" X: 나가기");
        }

        #endregion
        #region 중단 화면
        static void PrintStatus()
        {
            Console.SetCursorPosition(((data.foodMakeBar.x) * 2), (data.foodMakeBar.y) + 3);
            Console.Write($"소지 {data.burgurName}:{data.burgurCount}/{data.maxBurgurCount,-4}");
            Console.Write($"버거 품질: {data.quality.quality}");
            Console.SetCursorPosition(0, 6);
            Console.WriteLine("====================================================");
            Console.Write($"단계:{data.level,-5}");
            Console.Write($"exp:{data.exp}/{data.maxExp}\t");
            Console.Write($"돈:{data.money,-10}");
            Console.Write("평판:");
            for (int i = 0; i < ConvertStarToPopular(ref data.popularity); i++)
                Console.Write("★");
            Console.WriteLine();
            Console.WriteLine("====================================================");
        }
        #endregion
        #region 하단 화면
        static void PrintTextInterface()
        {
            Console.WriteLine("====================================================");
            if (data.isShop)
                PrintShop();
            else
                PrintGuest();
            Console.WriteLine("====================================================");
        }
        static void PrintShop()
        {
            int temp = 0;
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; i < (int)ShopItemName.SIZE / 2 + (int)ShopItemName.SIZE % 2; i++)
            {
                for (int j = i*2 ; j <=i*2+1; j++)
                {
                    if (j == (int)ShopItemName.SIZE)
                        break;
                    Console.SetCursorPosition(data.ShopChoicePoint.x + (j%2)*24, data.ShopChoicePoint.y+i*5);
                    if (j == (int)data.ShopMyChoice)
                    {
                        temp = j;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("▶ ");
                        Console.ResetColor();
                    }
                    else
                        Console.Write("▷ ");                                            
                    Console.Write($"{data.shopItem[j].name} {data.shopItem[j].price} G");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n\n\n");
            Console.WriteLine($"▶   {data.shopItem[temp].explanation}");
            Console.WriteLine();
            Console.WriteLine();
        }
        static void PrintGuest()
        {
            for (int i = 0; i < data.guest.Length; i++)
            {
                if (data.guest[i].isGuest == true)
                {
                    Console.WriteLine($"{data.guest[i].waitNumber}. {data.guest[i].name}");
                    Console.SetCursorPosition(data.gusetWaitCountPoint.x, data.gusetWaitCountPoint.y + i);
                    if (data.guest[i].waitingTime < 15)
                    {
                        Console.Write("인내심:");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{data.guest[i].waitingTime}/ 30");
                        Console.ResetColor();
                    }
                    else if (data.guest[i].waitingTime < 25)
                    {
                        Console.Write("인내심:");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{data.guest[i].waitingTime}/ 30");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("인내심:");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{data.guest[i].waitingTime}/ 30");
                        Console.ResetColor();
                    }
                }
            }
        }
        #endregion
        #endregion

        #region Input 관련
        static void InputKey()
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    MoveDown();
                    break;
                case ConsoleKey.LeftArrow:
                    MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    MoveRight();
                    break;
                case ConsoleKey.Z:
                    PressZ();
                    break;
                case ConsoleKey.X:
                    PressX();
                    break;
            }
        }
        static void InputKeyStove()
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Z:
                    PressZ();
                    break;
                case ConsoleKey.X:
                    PressX();
                    break;
            }
            return;
        }
        static void InputKeyShop()
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    ChooseUpShop();
                    break;
                case ConsoleKey.DownArrow:
                    ChooseDownShop();
                    break;
                case ConsoleKey.LeftArrow:
                    ChooseLeftShop();
                    break;
                case ConsoleKey.RightArrow:
                    ChooseRightShop();
                    break;
                case ConsoleKey.Z:
                    BuyItem();
                    break;
                case ConsoleKey.X:
                    PressX();
                    break;
            }
        }
        static void PressZ()
        {
            data.lastKey = ConsoleKey.Z;
            //상호작용 키
            switch (data.map[data.playerView.y, data.playerView.x])
            {
                case '♨':
                    OnStove();
                    break;
                case '☎':
                    OnShop();
                    break;
                case '▼':
                    Sellburgur();
                    break;
            }
        }
        static void PressX()
        {
            data.lastKey = ConsoleKey.X;
            OffStove();
            OffShop();
        }
        #endregion

        #region Update 관련
        #region Move
        static void MoveUp()
        {
            data.moveCount++;
            if (data.map[(data.playerPoint.y) - 1, (data.playerPoint.x)] == '□')
                data.playerPoint.y -= 1;
            data.playerView.x = data.playerPoint.x;
            data.playerView.y = data.playerPoint.y - 1;
            data.lastKey = ConsoleKey.UpArrow;
        }
        static void MoveDown()
        {
            data.moveCount++;
            if (data.map[(data.playerPoint.y) + 1, (data.playerPoint.x)] == '□')
                data.playerPoint.y += 1;
            data.playerView.x = data.playerPoint.x;
            data.playerView.y = data.playerPoint.y + 1;
            data.lastKey = ConsoleKey.DownArrow;
        }
        static void MoveLeft()
        {
            data.moveCount++;
            if (data.map[(data.playerPoint.y), ((data.playerPoint.x) - 1)] == '□')
                data.playerPoint.x -= 1;
            data.playerView.x = data.playerPoint.x - 1;
            data.playerView.y = data.playerPoint.y;
            data.lastKey = ConsoleKey.LeftArrow;
        }
        static void MoveRight()
        {
            data.moveCount++;
            if (data.map[(data.playerPoint.y), ((data.playerPoint.x) + 1)] == '□')
                data.playerPoint.x += 1;
            data.playerView.x = data.playerPoint.x + 1;
            data.playerView.y = data.playerPoint.y;
            data.lastKey = ConsoleKey.RightArrow;
        }
        #endregion        
        #region 스토브 관련
        static void OnStove()
        {
            if (data.isStove)
            {
                UseStove();
                return;
            }
            else
                data.isStove = true;
        }
        static void OffStove()
        {
            data.isStove = false;
            data.stoveCount = 0;
        }
        static int UseStove()
        {
            data.moveCount++;
            if (data.isStove)
            {
                data.stoveCount += data.cookSpeed.cookSpeed;
            }
            if (data.stoveCount > data.maxStoveCount)
            {              
                CountBurger(ref data.burgurCount,data.maxBurgurCount);
                data.stoveCount = 0;
                data.isStove = false;
            }
            return data.stoveCount;
        }
        static int CountBurger(ref int burgurCount,int maxBurgurCount)
        {          
            if (burgurCount >= maxBurgurCount)
                return maxBurgurCount;
            burgurCount += 1;
            return burgurCount;
        }
        #endregion
        #region 상점 관련
        #region 상점커서이동
        static void ChooseUpShop()
        {
            if ((int)data.ShopMyChoice <= 1)
                return;
            data.ShopMyChoice = (ShopItemName)((int)data.ShopMyChoice - 2);
        }
        static void ChooseDownShop()
        {
            if ((int)data.ShopMyChoice >= (int)ShopItemName.SIZE-2)
                return;
            data.ShopMyChoice = (ShopItemName)((int)data.ShopMyChoice + 2);
        }
        static void ChooseLeftShop()
        {
            if ((int)data.ShopMyChoice % 2 == 0)
                return;
            data.ShopMyChoice = (ShopItemName)((int)data.ShopMyChoice - 1);
        }
        static void ChooseRightShop()
        {
            if ((int)data.ShopMyChoice%2== 1 || ((int)data.ShopMyChoice + 1 == (int)ShopItemName.SIZE))
                return;
            data.ShopMyChoice = (ShopItemName)((int)data.ShopMyChoice + 1);
        }

        #endregion
        static void OnShop()
        {
            if (data.isShop)
            {
                return;
            }
            else
                data.isShop = true;
        }
        static void OffShop()
        {
            data.isShop = false;
        }
        static void BuyItem()
        {
            //현재 data.ShopItem 에 지정되있는 템 구입
            //값 만큼 차감
            //FastCook을 사면 cookSpeed가 1증가            
            if (data.ShopMyChoice == data.shopItem[0].name)
            {
                if (data.money < data.shopItem[0].price)
                    return;
                data.money -= data.shopItem[0].price;
                data.cookSpeed.cookSpeed++;
            }
            if (data.ShopMyChoice == data.shopItem[1].name)
            {
                if (data.money < data.shopItem[1].price)
                    return;
                data.money -= data.shopItem[1].price;
                data.maxBurgurCount++;
            }
            if (data.ShopMyChoice == data.shopItem[2].name)
            {
                if (data.money < data.shopItem[2].price)
                    return;
                data.money -= data.shopItem[2].price;
                data.quality.quality++;
            }
            if (data.ShopMyChoice == data.shopItem[4].name)
            {
                if (data.money < data.shopItem[4].price)
                    return;
                data.money -= data.shopItem[4].price;
                data.getPopularity +=data.popularUpgrade;
            }
        }

        #endregion
        #region 손님 관련
        static void CreateGuest()
        {
            //손님배열에 비어있는 맨 앞자리부터 손놈제작
            //손님이 배열에 들어오면 isConsumer true, 안들어오면 false;
            //손님은 먼저 10움직임에 한번씩 나오고 단계별로 나오는 손님이 많아진다.
            if (data.moveCount == 10)
            {
                data.waitingGuest += data.level;
                data.moveCount = 0;
            }
            for (int i = 0; i < data.guest.Length; i++)
            {
                if (data.waitingGuest == 0)
                    break;
                if (data.guest[i].isGuest == true)
                    continue;
                else
                {
                    data.guest[i].isGuest = true;
                    data.guest[i].name = (GuestName)random.Next(1,(int)GuestName.SIZE);
                    data.waitingGuest--;
                }
            }
        }
        static void Sellburgur()
        {
            //햄버거가 1개 이상이고 손님이 있을 때 개수 하나 차감
            //가장 앞에 손님 제거
            //그전 손님들 모두 앞으로 한칸씩
            if ((data.burgurCount > 0) && (data.guest[0].isGuest == true))
            {
                data.burgurCount--;
                //평판에 따른 돈 추가 보너스
                data.money += data.getMoney * (ConvertStarToPopular(ref data.popularity));
                data.popularity += data.getPopularity;
                data.exp++;
                data.guestCount++;
                ExitGuest();
            }
        }
        static void ExitGuest()
        {
            data.guest[0].isGuest = false;
            data.guest[0].name = GuestName.empty;
            data.guest[0].waitingTime = 0;
            for (int i = 0; i < data.guest.Length; i++)
            {
                if (i == data.guest.Length - 1)
                    break;
                data.guest[i].isGuest = data.guest[i + 1].isGuest;
                data.guest[i].name = data.guest[i + 1].name;
                data.guest[i].waitingTime = data.guest[i + 1].waitingTime;
            }
        }
        static void WaitTimeGuest()
        {
            if (data.isShop)
                return;
            for (int i = 0; i < data.guest.Length; i++)
            {
                if (data.guest[i].isGuest == true)
                    data.guest[i].waitingTime++;
            }
            while (data.guest[0].waitingTime > 30)
            {
                data.popularity -= 50;
                ExitGuest();
            }
        }
        #endregion
        #region 기타
        static int ConvertStarToPopular(ref int popularity)
        {
            if (popularity >= 500)
            {
                popularity = 500;
                return 5;
            }
            return popularity / 100;
        }
        static void levelUp()
        {
            if (data.exp >= data.maxExp)
            {
                data.level++;
                data.exp = 0;
            }
        }
        static bool LoseGame()
        {
            if (data.popularity <= 0)
                return data.running = false;
            return data.running = true;
        }
        static string UpgradeBurger()
        {
            if (data.quality.quality >= 2)
                data.burgurName = "존맛버거";
            return data.burgurName;
        }
        #endregion
        #endregion
    }
}
