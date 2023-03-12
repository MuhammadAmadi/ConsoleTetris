
bool YouCanMove(in char[,] field, in char[,] form, int yOs, int xOs, bool rotate = false)
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
    if (fullRowsNumber == string.Empty) fullRowsNumber = "0";

    return fullRowsNumber.Split(' ').Select(s => Int32.Parse(s)).ToArray();
}

char[,] DellFullRows(char[,] field, int[] fullRowsNumber)
{
    int count = 0;

    for (int i = field.GetLength(0) - 2, k = i; k >= 1; i--)
    {
        if (i == fullRowsNumber[count])
        {
            if (count < fullRowsNumber.Length - 1) count++;
            continue;
        }

        for (int j = 1; j < field.GetLength(1) - 1; j++)
        {
            if (i < 1) field[k, j] = ' ';
            else field[k, j] = field[i, j];

        }
        k--;
    }
    return field;
}

char[,] Rewrite(in char[,] copiedArray)
{
    char[,] rewritableArray = new char[copiedArray.GetLength(0), copiedArray.GetLength(1)];
    for (int i = 0; i < copiedArray.GetLength(0); i++)
    {
        for (int j = 0; j < copiedArray.GetLength(1); j++)
        {
            rewritableArray[i, j] = copiedArray[i, j];
        }
    }
    return rewritableArray;
}

char[,] Rotation(in char[,] form)
{
    char[,] temp = new char[form.GetLength(1), form.GetLength(0)];

    for (int i = 0; i < temp.GetLength(0); i++)
    {
        for (int j = 0; j < temp.GetLength(1); j++)
        {
            temp[i, j] = form[form.GetLength(0) - 1 - j, i];
        }
    }

    return temp;
}

char[,] Forms()
{
    Random rnd = new Random();
    char[,] form = new char[0, 0],

            form1 =
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

    switch (rnd.Next(0, 6))
    {
        case 0:
            for (int i = rnd.Next(2); i >= 0; i--) form = Rotation(form1);
            return form;
        case 1:
            return form2;
        case 2:
            for (int i = rnd.Next(2); i >= 0; i--) form = Rotation(form3);
            return form;
        case 3:
            for (int i = rnd.Next(2); i >= 0; i--) form = Rotation(form4);
            return form;
        case 4:
            for (int i = rnd.Next(4); i >= 0; i--) form = Rotation(form5);
            return form;
        case 5:
            for (int i = rnd.Next(4); i >= 0; i--) form = Rotation(form6);
            return form;
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

void Print(in char[,] field)
{
    Console.SetCursorPosition(0, 0);
    for (int i = 0; i < field.GetLength(0); i++)
    {
        for (int j = 0; j < field.GetLength(1); j++)
        {
            Console.Write(field[i, j]);
        }
        Console.WriteLine();
    }
}


//////////////////////////////////////////////////////////////////////////////////////

void Game(char[,] fieldDef)
{
    char[,] form = new char[0, 0],
            temp = new char[0, 0],
            field = new char[fieldDef.GetLength(0), fieldDef.GetLength(1)];
    bool check;
    int x = default,
        y = default,
        speedDef = 500,
        speed = speedDef,
        boost = 30;
    ConsoleKeyInfo key = new ConsoleKeyInfo();
    /////////////////////////////////////////////
    field = Rewrite(fieldDef);
    form = Forms();
    //////////////////////////////////////////////
    while (key.Key != ConsoleKey.Escape)
    {
        form = Forms();
        x = field.GetLength(1) / 2 - form.GetLength(1) / 2;
        y = default;
        check = true;
        while (check)
        {
            Thread.Sleep(speed);
            while (Console.KeyAvailable)
                key = Console.ReadKey(true);
            ////////////////////////////////
            switch (key.Key)
            {
                case ConsoleKey.A:
                    key = default;
                    if (x < 1)
                    {
                        x = 1;
                        goto default;
                    }
                    if (YouCanMove(field, form, y, x - 1))
                    {
                        x--;
                        speed = boost;
                    }
                    else
                        goto default;
                    break;
                case ConsoleKey.D:
                    key = default;
                    if (x > field.GetLength(1) - 2 - form.GetLength(1))
                    {
                        x = field.GetLength(1) - 1 - form.GetLength(1);
                        goto default;
                    }

                    if (YouCanMove(field, form, y, x + 1))
                    {
                        x++;
                        speed = boost;
                    }
                    else
                        goto default;
                    break;
                case ConsoleKey.W:
                    key = default;
                    temp = Rotation(form);
                    for (int i = 0; i < temp.GetLength(1); i++)
                    {
                        if (y > 0 && YouCanMove(field, temp, y, x - i, true))
                        {
                            form = Rewrite(temp);
                            speed = boost;
                            x = x - i;
                            break;
                        }
                    }
                    break;
                default:
                    check = YouCanMove(field, form, y + 1, x);
                    if (check)
                    {
                        y++;
                        speed = speedDef;
                    }
                    if (key.Key == ConsoleKey.S)
                    {
                        speed = speed / boost;
                        key = default;
                    }
                    break;
            }

            field = Move(field, form, y, x);
            Print(field);

            if (check) field = Move(field, form, y, x, check);
            else
            {
                int[] fullRowNum = FullRowsNumber(field, y, form.GetLength(0));
                if (fullRowNum[0] != 0)
                {
                    char[,] clear = new char[fullRowNum.Length, field.GetLength(1) - 2];
                    field = Move(field, clear, fullRowNum[0], 1, true);
                    Print(field);
                    field = DellFullRows(field, fullRowNum);
                    Thread.Sleep(50);
                    Print(field);
                }
            }

        }
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////

Console.CursorVisible = false;
Console.Clear();
int width = 18,
    height = 27;
char[,] field = new char[height, width];
/////////////////////////////////////////////
for (int i = 0; i < field.GetLength(0); i++)
{
    for (int j = 0; j < field.GetLength(1); j++)
    {
        if (i == 0 || j == 0 || i == field.GetLength(0) - 1 || j == field.GetLength(1) - 1) field[i, j] = '#';
        else field[i, j] = ' ';
    }
}
Game(field);
/////////////////////////////////////////////