export class Event {
    id!: number;
    date!: string;
    description!: string;
    type!: string;
    accountId?: number;
    amount?: number;
  }