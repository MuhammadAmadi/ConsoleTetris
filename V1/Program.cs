
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
    for (int i = 0; i < temp.GetLength(0); i++)
    {
        for (int j = 0; j < temp.GetLength(1); j++)
        {
            if(rotate)temp[i, j] = form[form.GetLength(0)-1-j, i];
            else temp[i, j] = form[i, j];
        }
    }

    return temp;
}

void Print(char[,] field)
{
    Console.Clear();
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
char[,] Field(in char[,] fd, in char[,] formDef)
{
    char[,] form = new char[formDef.GetLength(0), formDef.GetLength(1)],
            temp = new char[formDef.GetLength(0), formDef.GetLength(1)];
    for (int i = new Random().Next(1, 4); i > 0; i--)
    {
        form = Rotation(formDef);
    }
    char[,] field = new char[fd.GetLength(0), fd.GetLength(1)];
    Rewrite(ref field, fd);
    bool check = true;
    int x = field.GetLength(1) / 2 - form.GetLength(1) / 2,
        y = default,
        speedDef = 500,
        speed = speedDef;
    ConsoleKeyInfo key = new ConsoleKeyInfo();
    while (check)
    {

        Thread.Sleep(speed);
        while (Console.KeyAvailable)
            key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.A:
                if (YouCanMove(field, form, y, x - 1))
                {
                    x--;
                    speed = speed / 30;
                }
                else
                    goto default;
                break;

            case ConsoleKey.D:
                if (YouCanMove(field, form, y, x + 1))
                {
                    x++;
                    speed = speed / 30;
                }
                else
                    goto default;
                break;
            case ConsoleKey.S:
                check = YouCanMove(field, form, y + 1, x);
                if (check)
                {
                    y++;
                    speed = speed / 30;
                }
                break;
            case ConsoleKey.W:
                temp = Rotation(form);
                if (YouCanMove(field, temp, y, x))
                {
                    form = Rotation(temp, false);
                    speed = speed / 30;
                }
                break;
            default:
                check = YouCanMove(field, form, y + 1, x);
                if (check)
                {
                    y++;
                    speed = speedDef;
                }
                break;

        }
        key = default;

        Move(ref field, form, y, x);
        Print(field);

        if (check) Move(ref field, form, y, x, check);

    }
    return field;

}

////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////

int width = 18,
        height = 27;

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

for (int i = 0; i < field.GetLength(0); i++)
{
    for (int j = 0; j < field.GetLength(1); j++)
    {
        if (i == 0 || i == field.GetLength(0) - 1 || j == 0 || j == field.GetLength(1) - 1) field[i, j] = '#';
        else field[i, j] = ' ';
    }
}

//field = Field(field, form1);
field = Field(field, form2);
field = Field(field, form3);
field = Field(field, form4);
field = Field(field, form5);
field = Field(field, form6);


