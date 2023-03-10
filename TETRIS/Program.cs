
bool YouCanMove(in char[,] field, in char[,] form, int yOs, int xOs)
{
    for (int i = form.GetLength(0) - 1; i >= 0 && yOs > 0; i--, yOs--)
    {
        for (int j = 0; j < form.GetLength(1); j++)
        {
            if (form[i, j] != ' ')
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

void Rewrite(ref char[,] rewritableArray, in char[,] copiedArray)
{

    for (int i = 0; i < copiedArray.GetLength(0); i++)
    {
        for (int j = 0; j < copiedArray.GetLength(1); j++)
        {
            rewritableArray[i, j] = copiedArray[i, j];
        }
    }
}

void Move(ref char[,] field, in char[,] form, int yOs, int xOs, bool clear = false)
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
}

char[,] Rotation(in char[,] form, bool rotate = true)
{
    int x = 0, y = 1;
    if (!rotate)
    {
        x = 1; y = 0;
    }
    char[,] temp = new char[form.GetLength(y), form.GetLength(x)];
    /////////////////////////////////////////////////////////////
    for (int i = 0; i < temp.GetLength(0); i++)
    {
        for (int j = 0; j < temp.GetLength(1); j++)
        {
            if (rotate) temp[i, j] = form[form.GetLength(0) - 1 - j, i];
            else temp[i, j] = form[i, j];
        }
    }

    return temp;
}

void Print(in char[,] field)
{
    Console.SetCursorPosition(0,0);
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

char[,] Game(in char[,] fieldDef, in char[,] formDef)
{
    char[,] form = new char[0, 0],
            temp = new char[0, 0],
            field = new char[fieldDef.GetLength(0), fieldDef.GetLength(1)]; ;
    bool check = true;
    int x = field.GetLength(1) / 2 - form.GetLength(1) / 2,
        y = default,
        speedDef = 500,
        speed = speedDef,
        boost = 30;
    ConsoleKeyInfo key = new ConsoleKeyInfo();
    //////////////////////////////////////////////
    Rewrite(ref field, fieldDef);
    for (int i = new Random().Next(1, 5); i > 0; i--)
    {
        form = Rotation(formDef);
    }
    //////////////////////////////////////////////
    while (check)
    {
        Thread.Sleep(speed);
        while (Console.KeyAvailable)
            key = Console.ReadKey(true);
        ////////////////////////////////
        switch (key.Key)
        {
            case ConsoleKey.A:
                if (YouCanMove(field, form, y, x - 1))
                {
                    x--;
                    speed /= boost;
                }
                else
                    goto default;
                break;
            case ConsoleKey.D:
                if (YouCanMove(field, form, y, x + 1))
                {
                    x++;
                    speed /= boost;
                }
                else
                    goto default;
                break;
            case ConsoleKey.W:
                temp = Rotation(form);
                if (YouCanMove(field, temp, y, x))
                {
                    form = Rotation(temp, false);
                    speed /= boost;
                }
                break;
            default:
                check = YouCanMove(field, form, y + 1, x);
                if (check)
                {
                    y++;
                    speed = speedDef;
                }
                if (key.Key == ConsoleKey.S) speed = speed / boost;
                break;

        }
        ////////////////////////////////
        key = default;

        Move(ref field, form, y, x);
        Print(field);

        if (check) Move(ref field, form, y, x, check);

    }
    //////////////////////////////////////////////
    return field;

}

////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////
Console.CursorVisible = false;
Console.Clear();
int width = 18,
    height = 27,
    selectForm = default;
/////////////////////////////////////////////
char[,] field = new char[height, width],

form1 =
{
    {'0','0','0','0'}
},

form2 =
{
    {'0','0'},
    {'0','0'}
},

form3 =
{
    {'0','0',' '},
    {' ','0','0'}
},

form4 =
{
    {' ','0','0'},
    {'0','0',' '}
},

form5 =
{
    {'0',' ',' '},
    {'0','0','0'}
},

form6 =
{
    {' ',' ','0'},
    {'0','0','0'}
};
/////////////////////////////////////////////
for (int i = 0; i < field.GetLength(0); i++)
{
    for (int j = 0; j < field.GetLength(1); j++)
    {
        if (i == 0 || j == 0 || i == field.GetLength(0) - 1 || j == field.GetLength(1) - 1) field[i, j] = '#';
        else field[i, j] = ' ';
    }
}
/////////////////////////////////////////////
while (true)
{
    selectForm = new Random().Next(0, 6);
    switch (selectForm)
    {
        case 0:
            field = Game(field, form1);
            break;
        case 1:
            field = Game(field, form2);
            break;
        case 2:
            field = Game(field, form3);
            break;
        case 3:
            field = Game(field, form4);
            break;
        case 4:
            field = Game(field, form5);
            break;
        case 5:
            field = Game(field, form6);
            break;
    }
}
/////////////////////////////////////////////