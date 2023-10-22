using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RandomOrg.Application.Queries;
using RandomOrg.Domain.Repositories;
using RandomOrg.Infrastructure.Repositories;
using RandomOrg.Presentation;

namespace WinLottery
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            cboLottery.SelectedIndex = 0;
            app = App.GetApp(null);
        }
        IHost app;

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            btnGenerate.Enabled = false;

            int ticketsCount = (int)numTickets.Value;

            var lottery = app.Services.GetRequiredService<IRandomOrgLottery>();
            var tickets = cboLottery.SelectedIndex == 0 ?
                await lottery.GetTzokerTickets(ticketsCount) :
                await lottery.GetLottoTickets(ticketsCount);

            //var mediator = app.Services.GetRequiredService<IMediator>();
            //var tickets = await mediator.Send(new GetTzokerTicketsFromRandomOrgQuery(value));

            for (int i = 0; i < tickets.Count; i++)
                txtResults.AppendText($"Ticket #{i+1}: {tickets[i]}\r\n");

            btnGenerate.Enabled = true;
            Cursor.Current = Cursors.Default;
        }
    }
}