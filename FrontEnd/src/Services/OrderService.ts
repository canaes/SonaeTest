import {Order} from "../Types/Order";

export const fetchOrders = async (): Promise<Order[]> => {
  const response = await fetch('/api/orders');
  return response.json();
};

export const createOrder = async (quantity: number): Promise<boolean> => {
  const response = await fetch('/api/orders', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ quantity }),
  });
  return response.ok;
};

export const completeOrder = async (orderId: string): Promise<boolean> => {
  const response = await fetch(`/api/orders/${orderId}/complete`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
  });
  return response.ok;
};
