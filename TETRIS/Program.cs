
bool CheckCanMove(in char[,] field, in char[,] form, int yOs, int xOs, bool rotate = false)
{
    for (int i = form.GetLength(0) - 1; i >= 0 && yOs > 0; i--, yOs--)
    {
        for (int j = 0; j < form.GetLength(1); j++)
        {
            if (rotate || form[i, j] != ' ')
            {
                if (field[yOs, xOs + j] != ' ')
                {
                    return false;
                }
            }
        }
    }
    return true;
}

int[] FullRowsNumber(in char[,] field, int yOs, int heightForm)
{
    bool full = false;
    string fullRowsNumber = string.Empty;
    for (; heightForm > 0 && yOs >= 0; yOs--, heightForm--)
    {
        for (int j = 1; j < field.GetLength(1) - 1; j++)
        {
            full = true;
            if (field[yOs, j] == ' ')
            {
                full = false;
                break;
            }

        }
        if (fullRowsNumber != string.Empty && full) fullRowsNumber += $" {yOs}";
        else if (full) fullRowsNumber += $"{yOs}";
    }
    if (fullRowsNumber == string.Empty) fullRowsNumber = "-1";

    return fullRowsNumber.Split(' ').Select(s => Int32.Parse(s)).ToArray();
}

char[,] Rewrite(in char[,] fieldDef, in int[]? fullRowsNumber = default, bool dellFullRows = false)
{
    char[,] field = new char[fieldDef.GetLength(0), fieldDef.GetLength(1)];
    int count = 0;
    for (int i = field.GetLength(0) - 1, k = i; k >= 0; i--)
    {
        if (dellFullRows && count < fullRowsNumber?.Length && i == fullRowsNumber[count])
        {
            count++;
            continue;
        }

        for (int j = 0; j < field.GetLength(1); j++)
        {
            if (dellFullRows)
            {
                if (i >= 1) field[k, j] = fieldDef[i, j];
                else if (k > 0 && j > 0 && j < field.GetLength(1) - 1) field[k, j] = ' ';
                else field[k, j] = fieldDef[0, j];
            }
            else field[i, j] = fieldDef[i, j];
        }
        k--;
    }
    return field;
}

char[,] Rotation(in char[,] form)
{
    char[,] temp = new char[form.GetLength(1), form.GetLength(0)];

    for (int i = 0; i < temp.GetLength(0); i++)
    {
        for (int j = 0; j < temp.GetLength(1); j++)
            temp[i, j] = form[form.GetLength(0) - 1 - j, i];
    }

    return temp;
}

char[,] Forms()
{
    Random rnd = new Random((int)DateTime.Now.Ticks);

    char[,] form1 =
            {
                { '0','0','0','0'}
            },

            form2 =
            {
                { '0','0'},
                { '0','0'}
            },

            form3 =
            {
                { '0','0',' '},
                { ' ','0','0'}
            },

            form4 =
            {
                { ' ','0','0'},
                { '0','0',' '}
            },

            form5 =
            {
                { '0',' ',' '},
                { '0','0','0'}
            },

            form6 =
            {
                { ' ',' ','0'},
                { '0','0','0'}
            };

    switch (rnd.Next(6))
    {
        case 0:
            for (int i = rnd.Next(3); i >= 0; i--) form1 = Rotation(form1);
            return form1;
        case 1:
            return form2;
        case 2:
            for (int i = rnd.Next(3); i >= 0; i--) form3 = Rotation(form3);
            return form3;
        case 3:
            for (int i = rnd.Next(3); i >= 0; i--) form4 = Rotation(form4);
            return form4;
        case 4:
            for (int i = rnd.Next(5); i >= 0; i--) form5 = Rotation(form5);
            return form5;
        case 5:
            for (int i = rnd.Next(5); i >= 0; i--) form6 = Rotation(form6);
            return form6;
        default:
            return form1;
    }
}

char[,] Move(char[,] field, in char[,] form, int yOs, int xOs, bool clear = false)
{
    for (int i = form.GetLength(0) - 1; i >= 0 && yOs > 0; i--, yOs--)
    {
        for (int j = 0; j < form.GetLength(1); j++)
        {
            if (form[i, j] != ' ')
            {
                if (clear) field[yOs, xOs + j] = ' ';
                else field[yOs, xOs + j] = form[i, j];
            }
        }
    }
    return field;
}

void Print(in char[,] field, in char[,] nextForm, in int score, in int level)
{
    string[] sidebar =
    {
        $"Score: {score}\t",$"Level: {level}"," ","Next:"
    };

    Console.SetCursorPosition(0, 0);
    for (int i = 0; i < field.GetLength(0); i++)
    {
        for (int j = 0; j < field.GetLength(1); j++)
            Console.Write(field[i, j]);

        if (i < sidebar.Length)
        {
            Console.SetCursorPosition(field.GetLength(1) + 1, i);
            Console.Write(sidebar[i]);
            Console.WriteLine();
            continue;
        }
        if (i < nextForm.GetLength(0) + sidebar.Length)
        {
            Console.SetCursorPosition(field.GetLength(1) + 1, i);
            for (int j = 0; j < nextForm.GetLength(1); j++)
                Console.Write(nextForm[i - sidebar.Length, j]);
        }
        Console.WriteLine();
    }
}

//////////////////////////////////////////////////////////////////////////////////////

void Game(char[,] field)
{
    char[,] form = new char[0, 0], // хранит игровую фигуру
            nextForm = new char[0, 0], // хранит игровую фигуру
            temp = new char[0, 0];
    bool canMove;
    int x = default,
        y = default,
        level = default,
        score = default,
        speedDef = 500,
        speed = speedDef,
        boost = 30;
    ConsoleKeyInfo key = new ConsoleKeyInfo(); // переменная для хранения нажатой клавиши
    /////////////////////////////////////////////
    form = Forms(); // вызываем метод для выбора игровой фигуры
    //////////////////////////////////////////////
    while (true)
    {
        //////////////////////////////////////////////////
        nextForm = Forms(); //вызываем метод для выбора следующей игровой фигуры
        x = field.GetLength(1) / 2 - form.GetLength(1) / 2; // определяем относительную середину
        y = default;
        canMove = true;
        //////////////////////////////////////////////////
        while (canMove)
        {
            Thread.Sleep(speed); // таймер задержки
            while (Console.KeyAvailable) // проверяем была ли нажата клавиша
                key = Console.ReadKey(true); // если была нажата присваеиваем переменной key значение нажатой клавиши
            /////////////////////////////////////////////////
            switch (key.Key) // выбираем действие соответсвующий нажатой клавиши
            {
                case ConsoleKey.P:
                    Console.ReadKey(true);
                    key = default;
                    break;
                case ConsoleKey.Escape:
                    return;
                /////////////////////////////////////////////    
                case ConsoleKey.A:
                    key = default; // сбрасываем значение переменной после того как она отработала
                    if (x < 1) // возврашаем значение x чтобы не вышло за поле
                    {
                        x = 1;
                        goto default; // идем в кейс по умолчанию чтобы продолжать движение вниз
                    }
                    if (CheckCanMove(field, form, y, x - 1)) //проверяем можно ли двигатся в том направлении по которой хотим
                    {
                        x--; // если можно двигаться уменьшаем значение х
                        speed = boost; // меняем время задержки таймера
                    }
                    else
                        goto default;
                    break;
                case ConsoleKey.D:
                    key = default;
                    if (x > field.GetLength(1) - 2 - form.GetLength(1)) // возврашаем значение x чтобы не вышло за поле
                    {
                        x = field.GetLength(1) - 1 - form.GetLength(1);
                        goto default;
                    }
                    if (CheckCanMove(field, form, y, x + 1)) //проверяем можно ли двигатся в том направлении по которой хотим
                    {
                        x++; // если можно двигаться увеличиваем значение х
                        speed = boost;
                    }
                    else
                        goto default;
                    break;
                case ConsoleKey.W:
                    key = default;
                    temp = Rotation(form); // вызываем метод врашения игровой фигуры
                    for (int i = 0; i < temp.GetLength(1); i++) // это сделано чтобы фигура могла врашаться если прижата к правой стенке
                    {
                        if (y > 0 && CheckCanMove(field, temp, y, x - i, true))
                        {
                            form = Rewrite(temp);
                            speed = boost;
                            x = x - i;
                            break;
                        }
                    }
                    break;
                default:
                    canMove = CheckCanMove(field, form, y + 1, x); // проверяет можно двигаться вниз, если нельзя canMove = false
                    if (canMove)
                    {
                        y++;
                        speed = speedDef;
                    }
                    if (key.Key == ConsoleKey.S) // ускорение вниз
                    {
                        speed = speed / boost;
                        key = default;
                    }
                    break;
            }
            /////////////////////////////////////////////////
            field = Move(field, form, y, x); // записываем в поле фигуру
            Print(field, nextForm, score, level); // печатаем поле

            if (canMove) field = Move(field, form, y, x, canMove); // если canMove = true удаляем фигуру который нарисовали ранее
            else
            {
                if (y < form.GetLength(0)) // если y < высоты фигуры GameOver
                {
                    char[,] gameOver =
                    {
                        {'#','#','#','#','#','#'},
                        {'#','G','A','M','E','#'},
                        {'#','#','#','#','#','#'},
                        {'#','O','V','E','R','#'},
                        {'#','#','#','#','#','#'}
                    };

                    field = Move(field, gameOver, field.GetLength(0) / 2 + 2, field.GetLength(1) / 2 - 3);
                    Print(field, nextForm, score, level);
                    return;
                }
                score++;
                int[] fullRowNum = FullRowsNumber(field, y, form.GetLength(0)); // вызываем метод для проверки заполнен ли какая либо строка, по умолчанию возхврашает -1, если заполнено строка(и) возврашает индекс строк(и)
                if (fullRowNum[0] >= 0) // если значение в первом элементо 0 или больше удаляем строки
                {
                    char[,] clear = new char[1, field.GetLength(1) - 2]; // создаем пустую матрицу на одну строку 
                    for (int i = 0; i < fullRowNum.Length; i++) // с помощю массива fullRowNum определяется сколько строк нужно удалить
                        field = Move(field, clear, fullRowNum[i], 1, true); // записываем в поле пустую фигуру
                    Print(field, nextForm, score, level); // печатаем
                    field = Rewrite(field, fullRowNum, true); // теперь удаляем строки которые очистили ранее
                    Thread.Sleep(speed); 
                    Print(field, nextForm, score, level); // печатаем
                    level += fullRowNum.Length; // увеличиваем уровень на количество удаленных строк
                }
                char[,] delOldForm =
                {
                    {' ',' ',' ',' '},
                    {' ',' ',' ',' '},
                    {' ',' ',' ',' '},
                    {' ',' ',' ',' '}
                };
                Print(field, delOldForm, score, level); // очишаем поле где показывала следующую фигуру
                form = nextForm; // следующая фигура становится игровой
            }
        }
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////
//START
Console.CursorVisible = false; // отключает курсор на консоли
Console.Clear(); // очишает консоль
int width = 20,     // ширина поля
    height = 28;    // высота поля
char[,] field = new char[height, width]; // массив поля
/////////////////////////////////////////////
/// задаем границы поля
for (int i = 0; i < field.GetLength(0); i++)
{
    for (int j = 0; j < field.GetLength(1); j++)
    {
        if (i == 0 || j == 0 || i == field.GetLength(0) - 1 || j == field.GetLength(1) - 1) field[i, j] = '#';
        else field[i, j] = ' ';
    }
}
Game(field);
//END