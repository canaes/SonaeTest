import React, { useState } from 'react';
import { createOrder } from '../../Services/OrderService.ts';
import './style.css';

interface CreateOrderModalProps {
  onClose: () => void;
  onSubmitSuccess: () => void;
}

const CreateOrderModal: React.FC<CreateOrderModalProps> = ({ onClose, onSubmitSuccess }) => {
  const [quantity, setQuantity] = useState<number>(1);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const success = await createOrder(quantity);
    if (success.success) {
      alert('Encomenda criada com sucesso!');
      setQuantity(1);
      onSubmitSuccess();
      onClose();
    } else {
      alert(`Erro ao criar encomenda. [${Array(success.errors).join(', ')}]`);
    }
  };

  return (
    <div>
      <div className="modal-background" onClick={onClose} />
      <div className="modal-content">
        <h2>Criar Nova Encomenda</h2>
        <form onSubmit={handleSubmit} style={{marginTop: '5vh', padding: '1vh'}}>
          <label>
            Quantidade:
            <input
              className='input-plus'
              type="number"
              value={quantity}
              onChange={(e) => setQuantity(Number(e.target.value))}
              min="1"
              max="100"
              style={{marginLeft: '16px'}}
            />
          </label>
          <button type="submit" style={{marginLeft: '16px'}}>Submeter Encomenda</button>
        </form>
      </div>
    </div>
  );
};

export default CreateOrderModal;
